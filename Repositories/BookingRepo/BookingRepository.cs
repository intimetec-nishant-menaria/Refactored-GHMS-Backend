using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public class BookingRepository : IBookingRepository
    {
        public readonly DBContext _context;

        public BookingRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r =>
                    !r.Bookings
                       .Any(b =>
                            b.Status != BookingStatusEnum.Cancelled &&
                            b.Status != BookingStatusEnum.Completed &&
                            (b.CheckOutDate > roomAvaiblityRequest.checkIn ||
                            b.CheckInDate < roomAvaiblityRequest.checkOut )
            )
                ).Select(room => new RoomResponseDto
                {
                    Id = room.Id,
                    RoomNumber = room.RoomNumber,
                    RoomTypeId = room.RoomTypeId,
                    RoomTypeName = room.RoomType.RoomTypeName.ToString(),
                    Capacity = room.RoomType.Capacity,
                    PricePerNight = room.RoomType.PricePerNight,
                    RoomStatus =room.RoomStatus
                }).ToListAsync();
        }
        public async Task<IEnumerable<BookingResponseDto>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Select(b=>new BookingResponseDto
                {
                    BookingId = b.Id,
                    UserId = b.User.Id,
                    UserEmail = b.User.Email,
                    RoomId = b.RoomId,
                    RoomNumber  =   b.Room.RoomNumber,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    Status = b.Status,
                })
                .ToListAsync();
        }
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }
        public async Task UpdateAsync(Booking booking)
        {
             _context.Bookings.Update(booking);
        }
        public async Task DeleteAysnc(Booking booking)
        {
            _context.Bookings.Remove(booking);
        }
        public async Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut,
            int? excludeBookingId = null)
        {
            var res = !await _context.Bookings
                .AnyAsync(b =>
                    b.RoomId == roomId &&
                    b.Status != BookingStatusEnum.Cancelled &&
                    b.Status != BookingStatusEnum.Completed &&
                    b.CheckOutDate > CheckIn &&
                    b.CheckInDate < CheckOut);

            return res;
        }

        public async Task<IEnumerable<CalendarEventResponceDto>> fetchByRange(DateTime start ,DateTime end)
        {
            var res= await _context.Bookings.Where(
                b =>
                    b.CheckInDate < end &&
                    b.CheckOutDate > start
            ).Select( b=> new CalendarEventResponceDto
            {
                BookingId = b.Id,
                UserName = b.User.Name,
                RoomNumber = b.Room.RoomNumber,
                Start = b.CheckInDate,
                End = b.CheckOutDate,
                BookingStatus = b.Status
            }).ToListAsync();

            return res;
        }
    }
}
