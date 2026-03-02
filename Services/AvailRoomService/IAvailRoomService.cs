using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.AvailRoomService
{
    public interface IAvailRoomService
    {
        Task<List<AvailableRoomDto>> GetAvailableRoomsAsync(AvailabilityRequestDto request);
    }
}
