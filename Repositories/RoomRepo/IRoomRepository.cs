using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.RoomRepo
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);
        Task<IEnumerable<RoomResponseDto>> GetAllRoomsAsync();
        Task<bool> UpdateRoomStatusAsync(int id, Enums.RoomStatusEnum status);
        Task<object> GetRoomStatusSummaryAsync();
        Task AddAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task<bool> RoomNumberExistsAsync(string roomNumber);
        Task<bool> DeleteRoomByIdAsync(int id);
    }
}
