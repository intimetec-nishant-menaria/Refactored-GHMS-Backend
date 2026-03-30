using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.AvailableRoomRepo
{
    public interface IAvailRoomRepository
    {
        Task<IEnumerable<RoomResponseDto>> GetAvailableRoomAsync(GenderEnum gender,DateTime checkIn, DateTime checkOut);
    }
}
