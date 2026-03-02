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
                throw new Exception("Check-out must be after check-in.");
            }

            if (request.CheckIn.Date < DateTime.UtcNow.Date)
            {
                throw new Exception("Check-in cannot be in the past.");
            }

            var normalizedCheckIn = request.CheckIn.Date.AddHours(14); 
            var normalizedCheckOut = request.CheckOut.Date.AddHours(11); 

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var rooms = await _roomRepository
                    .GetAvailableRoomAsync(normalizedCheckIn, normalizedCheckOut);

                var nights = (request.CheckOut.Date - request.CheckIn.Date).Days;

                var result = rooms.Select(r => new AvailableRoomDto
                {
                    RoomId = r.Id,
                    RoomNumber = r.RoomNumber,
                    PricePerNight = r.RoomType.PricePerNight,
                    TotalPrice = nights * r.RoomType.PricePerNight
                }).ToList();

                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
