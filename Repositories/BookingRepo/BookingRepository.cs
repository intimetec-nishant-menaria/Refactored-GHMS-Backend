using guest_house_management_backend.Data;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using guest_house_management_backend.DTOs;
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
        public async Task<List<BookingResponseDto>> GetAllAsync()
        {
            return await _context.Bookings
            .Include(b => b.Guest)
            .Include(b => b.Room)
            .Select(b => new BookingResponseDto
            {
                Id = b.Id,
                GuestId = b.GuestId,
                GuestName = b.Guest.Name,
                GuestEmail = b.Guest.Email,
                RoomId = b.RoomId,
                RoomNumber = b.Room.RoomNumber,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Status = b.Status,
                price = b.price,
                SpecialRequests = b.SpecialRequests
            })
            .ToListAsync();
        }
        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Where(b => b.Id == id)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    GuestId = b.GuestId,
                    GuestName = b.Guest.Name,
                    RoomId = b.RoomId,
                    RoomNumber = b.Room.RoomNumber,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    Status = b.Status,
                    price = b.price,
                    SpecialRequests = b.SpecialRequests

                })
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
                    ThenInclude(r=>r.RoomType).
                    Include(b => b.Guest).FirstOrDefaultAsync(b=>b.Id == bookingId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
            var res= await _context.Bookings.Include(b=>b.Guest).Where(
                b =>
                    b.CheckInDate < end &&
                    b.CheckOutDate > start
            ).Select( b=> new CalendarEventResponceDto
            {
                BookingId = b.Id,
                UserName = b.Guest.Name,
                RoomNumber = b.Room.RoomNumber,
                Start = b.CheckInDate,
                End = b.CheckOutDate,
                BookingStatus = b.Status
            }).ToListAsync();

            return res;
        }

        public async Task<Booking?> getBookingById(int id)
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
