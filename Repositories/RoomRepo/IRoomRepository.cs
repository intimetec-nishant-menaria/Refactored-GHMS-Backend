using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.RoomRepo
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);
        Task<bool> UpdateRoomStatusAsync(int id, RoomStatusEnum status);
        Task<object> GetRoomStatusSummaryAsync();
    }
}
