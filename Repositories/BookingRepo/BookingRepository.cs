using guest_house_management_backend.Data;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using guest_house_management_backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public class BookingRepository:IBookingRepository
    {
        private readonly DBContext _context;

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
        public async Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut,
            int? excludeBookingId = null)
        {
            return await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.Status != BookingStatusEnum.Cancelled &&
                            (!excludeBookingId.HasValue || b.Id != excludeBookingId.Value) &&
                            CheckIn < b.CheckOutDate &&
                            CheckOut > b.CheckInDate)
                .AnyAsync();
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId);
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
                    !r.Bookings.Any(
                        b => 
                            b.CheckInDate < roomAvaiblityRequest.checkOut &&
                            b.CheckOutDate > roomAvaiblityRequest.checkIn
                    )
                ).Select(room => new RoomResponseDto
                {
                    Id = room.Id,
                    RoomNumber = room.RoomNumber,
                    RoomTypeId = room.RoomTypeId,
                    RoomTypeName = room.RoomType.RoomTypeName.ToString(),
                    Capacity = room.RoomType.Capacity,
                    PricePerNight = room.RoomType.PricePerNight,
                    RoomStatus = room.RoomStatus
                }).ToListAsync();
        }
    }
}
