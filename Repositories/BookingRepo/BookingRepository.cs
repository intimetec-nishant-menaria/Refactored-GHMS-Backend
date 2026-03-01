using guest_house_management_backend.Data;
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
        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .ToListAsync();
        }
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Guest)
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
            return await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                            b.Status != BookingStatusEnum.Cancelled &&
                            (!excludeBookingId.HasValue || b.Id != excludeBookingId.Value) &&
                            CheckIn < b.CheckOutDate &&
                            CheckOut > b.CheckInDate)
                .AnyAsync();
        }
    }
}
