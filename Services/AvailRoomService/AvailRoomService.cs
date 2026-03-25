using guest_house_management_backend.DTOs;
using guest_house_management_backend.Repositories.AvailableRoomRepo;

namespace guest_house_management_backend.Services.AvailRoomService
{
    public class AvailRoomService : IAvailRoomService
    {
        private readonly IAvailRoomRepository _roomRepository;

        public AvailRoomService( IAvailRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(AvailabilityRequestDto request)
        {
            if (request.CheckIn > request.CheckOut)
            {
                throw new InvalidOperationException("Check-out must be after check-in.");
            }

            return await _roomRepository.GetAvailableRoomAsync(request.CheckIn, request.CheckOut);
        }
    }
}
