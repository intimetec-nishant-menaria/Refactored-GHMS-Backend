using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Repositories.AvailableRoomRepo;

namespace guest_house_management_backend.Services.AvailRoomService
{
    public class AvailRoomService : IAvailRoomService
    {
        private readonly DBContext _context;
        private readonly IAvailRoomRepository _roomRepository;

        public AvailRoomService(DBContext context, IAvailRoomRepository roomRepository)
        {
            _context = context;
            _roomRepository = roomRepository;
        }

        public async Task<List<AvailableRoomDto>> GetAvailableRoomsAsync(AvailabilityRequestDto request)
        {
            if (request.CheckIn >= request.CheckOut)
            {
                throw new InvalidOperationException("Check-out must be after check-in.");
            }

            if (request.CheckIn.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException("Check-in cannot be in the past.");
            }

            var normalizedCheckIn = request.CheckIn.Date.AddHours(14);
            var normalizedCheckOut = request.CheckOut.Date.AddHours(11);

            var rooms = await _roomRepository
                .GetAvailableRoomAsync(normalizedCheckIn, normalizedCheckOut);

            var nights = (request.CheckOut.Date - request.CheckIn.Date).Days;

            var result = rooms.Select(r =>
                {
                    var price = r.RoomType?.PricePerNight ?? 0;

                    return new AvailableRoomDto
                    {
                        RoomId = r.Id,
                        RoomNumber = r.RoomNumber,
                        PricePerNight = price,
                        TotalPrice = nights * price
                    };
                }).ToList();

            return result;
        }
    }
}
