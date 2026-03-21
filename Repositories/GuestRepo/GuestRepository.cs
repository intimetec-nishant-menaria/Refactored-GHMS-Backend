using AutoMapper;
using AutoMapper.QueryableExtensions;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.GuestRepo
{
    public class GuestRepository : IGuestRepository
    {
        private readonly Data.DBContext _context;
        private readonly IMapper _mapper;

        public GuestRepository(Data.DBContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Paging<GuestResponseDto>> GetAllAsync(int pageNumber , int pageSize , string searchUser)
        {
            var query = _context.Guest.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchUser))
            {
                query = query.Where(g => g.Email.Contains(searchUser));
            }
            var totalCount = await query.CountAsync();

            var res = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<GuestResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new Paging<GuestResponseDto>
            {
                Data = res,
                MetaData =
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber
                }
            };
        }

        public async Task<Guest?> GetByIdAsync(int guestId)
        {
            return await _context.Guest.FindAsync(guestId);
        }

        public async Task<Guest?> GetByEmailAsync(string email)
        {
            return await _context.Guest.FirstOrDefaultAsync(g => g.Email == email);
        }

        public async Task<List<Guest>> SearchAsync(string search)
        {
            return await _context.Guest
                .Where(g => g.Name.Contains(search)
                         || g.Email.Contains(search))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsDuplicateAsync(string email, string contact)
        {
            return await _context.Guest
                .AnyAsync(g => g.Email == email || g.Contact == contact);
        }

        public async Task AddAsync(Guest guest)
        {
            await _context.Guest.AddAsync(guest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guest guest)
        {
            _context.Guest.Update(guest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guest guest)
        {
            _context.Guest.Remove(guest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetGuestBookings(int guestId)
        {
            return await _context.Bookings
                .Where(b => b.GuestId == guestId)
                .Include(b => b.Room)
                .ToListAsync();
        }
    }
}
