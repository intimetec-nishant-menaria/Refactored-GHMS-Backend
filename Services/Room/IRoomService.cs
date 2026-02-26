using guest_house_management_backend.Enums;

namespace guest_house_management_backend.Services.Room
{
    public interface IRoomService
    {
        Task<bool> UpdateRoomStatusAsync(int id, RoomStatusEnum status);
        Task<RoomStatusEnum?> GetRoomStatusAsync(int id);
        Task<object> GetRoomStatusSummaryAsync();
    }
}
