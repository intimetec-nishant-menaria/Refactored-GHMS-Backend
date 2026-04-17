using AutoMapper;
using AutoMapper.QueryableExtensions;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly Data.DBContext _context;
        private readonly IMapper _mapper;

        public UserRepository(Data.DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                    .Include(u => u.Role)   
                    .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Paging<UserResponseDto>> GetAllAsync(int pageNumber , int pageSize , string searchUser)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchUser))
            {
                query = query.Where(u => u.Email.Contains(searchUser));
            }

            var totalCount = await query.CountAsync();
            var res = await query.Skip((pageNumber-1)*pageSize).Take(pageSize)
                .ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new Paging<UserResponseDto>
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

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return;
            }

            _context.Users.Remove(user);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
