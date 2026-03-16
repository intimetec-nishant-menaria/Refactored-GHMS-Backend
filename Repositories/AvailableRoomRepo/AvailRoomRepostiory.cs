using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
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
        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomAsync(DateTime checkIn, DateTime checkOut)
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r =>
                    !r.Bookings
                       .Any(b =>
                            b.Status != BookingStatusEnum.Cancelled &&
                            b.Status != BookingStatusEnum.Completed &&
                            (b.CheckOutDate >= checkIn ||
                            b.CheckInDate <= checkOut)
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
