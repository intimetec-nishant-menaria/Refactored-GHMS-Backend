using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.AvailRoomService
{
    public interface IAvailRoomService
    {
        Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(AvailabilityRequestDto request);
    }
}
