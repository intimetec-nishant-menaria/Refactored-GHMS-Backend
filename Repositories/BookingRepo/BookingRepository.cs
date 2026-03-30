using guest_house_management_backend.Data;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using guest_house_management_backend.DTOs;
using Microsoft.EntityFrameworkCore;
using guest_house_management_backend.DTOs.Paging;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public class BookingRepository : IBookingRepository
    {
        public readonly DBContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(DBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Paging<BookingResponseDto>> GetAllAsync(int pageNumber , int pageSize , string searchUser , string roomNumber ,int statusFilter)
        {
            var query = _context.Bookings.Include(b => b.Room).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchUser))
            {
                query = query.Where(b => b.GuestEmail.Contains(searchUser));
            }

            if (!string.IsNullOrWhiteSpace(roomNumber))
            {
                query = query.Where(b => b.Room.RoomNumber.Contains(roomNumber));
            }

            if (statusFilter != 0)
            {
                query = query.Where(b => b.Status == (Enums.BookingStatusEnum)statusFilter);
            }

            var totalCount = await query.CountAsync();
            var res =await query.OrderBy(b=>b.Status).ThenByDescending(b=>b.CheckInDate).Skip((pageNumber - 1)*pageSize).Take(pageSize)
            .ProjectTo<BookingResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            return new Paging<BookingResponseDto>
            {
                Data = res,
                MetaData =
                {
                    TotalCount = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                }
            };
        }
        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }
        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAysnc(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if(booking == null)
            {
                return;
            }
            _context.Bookings.Remove(booking);
            await SaveChangesAsync();
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings.
                    Include(b=>b.Room).
                    FirstOrDefaultAsync(b=>b.Id == bookingId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            return await _context.Rooms
                .Where(r =>
                    !r.Bookings
                       .Any(b =>
                            b.Status != BookingStatusEnum.Cancelled &&
                            b.Status != BookingStatusEnum.Completed &&
                            (b.CheckOutDate > roomAvaiblityRequest.checkIn ||
                            b.CheckInDate < roomAvaiblityRequest.checkOut )
            )
                ).ProjectTo<RoomResponseDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task UpdateBookingAsync(Booking booking)
        {
             _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut,
            int? excludeBookingId = null)
        {
            var res = await _context.Rooms
                .Where(r=>r.Id == roomId)
                .AnyAsync(
                    r=>r.Bookings.Count(b =>
                        b.Id != excludeBookingId &&
                        b.Status != BookingStatusEnum.Cancelled &&
                        b.Status != BookingStatusEnum.Completed &&
                        b.CheckInDate < CheckOut &&
                        b.CheckOutDate > CheckIn) < 2);

            return res;
        }

        public async Task<IEnumerable<BookingResponseDto>> fetchByRange(DateTime start ,DateTime end)
        {
            var res= await _context.Bookings
                .Include(b => b.Room)
                .Where(
                    b =>
                        b.CheckInDate < end &&
                        b.CheckOutDate > start
                ).ProjectTo<BookingResponseDto>(_mapper.ConfigurationProvider).ToListAsync();

            return res;
        }

        public async Task<Booking?> getBookingById(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Paging<BookingResponseDto>> GetUserBookings(int pageNumber, int pageSize, string guestEmail , string roomNumber , int statusFilter)
        {
            var query =  _context.Bookings.Where(b => b.GuestEmail == guestEmail).AsQueryable();

            if (!string.IsNullOrWhiteSpace(roomNumber)){
                query = query.Where(b => b.Room.RoomNumber.Contains(roomNumber));
            }

            if (statusFilter != 0)
            {
                query = query.Where(b => b.Status == (Enums.BookingStatusEnum)statusFilter);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(b => b.CheckInDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BookingResponseDto>(_mapper.ConfigurationProvider).ToListAsync();

            return new Paging<BookingResponseDto>
            {
                Data = items,
                MetaData =
                {
                    TotalCount = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                }
            };
        }
    }
}
