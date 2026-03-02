using guest_house_management_backend.Data;
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
