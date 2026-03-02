using guest_house_management_backend.Data;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.AvailableRoomRepo
{
    public class AvailRoomRepostiory : IAvailRoomRepository
    {
        private readonly DBContext _context;
        public AvailRoomRepostiory(DBContext context)
        {
            _context = context;
        }
        public async Task<List<Room>> GetAvailableRoomAsync(DateTime checkIn, DateTime checkOut)
        {
            var bookedRoomIds = await _context.Bookings
                 .Where(b =>
                     b.Status != Enums.BookingStatusEnum.Cancelled &&
                     b.Status != Enums.BookingStatusEnum.CheckedOut &&
                     b.CheckInDate < checkOut &&
                     b.CheckOutDate > checkIn)
                 .Select(b => b.RoomId)
                 .ToListAsync();
            return await _context.Room
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
