using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
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

        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(GenderEnum gender , DateTime checkIn , DateTime checkOut)
        {
            if ( checkIn > checkOut)
            {
                throw new InvalidOperationException("Check-out must be after check-in.");
            }

            return await _roomRepository.GetAvailableRoomAsync( gender, checkIn, checkOut);
        }
    }
}
