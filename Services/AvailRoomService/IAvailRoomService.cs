using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;

namespace guest_house_management_backend.Services.AvailRoomService
{
    public interface IAvailRoomService
    {
        Task<IEnumerable<RoomResponseDto>> GetAvailableRoomsAsync(GenderEnum gender , DateTime checkIn , DateTime checkOut);
    }
}
