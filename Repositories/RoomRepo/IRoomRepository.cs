using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.RoomRepo
{
    public interface IRoomRepository
    {
        Task<Room?> GetRoomByIdAsync(int id);
        Task<Paging<RoomResponseDto>> GetAllRoomsAsync(int pageNumber , int pageSize , int roomStatus , string roomNumber);
        Task<bool> UpdateRoomStatusAsync(int id, Enums.RoomStatusEnum status);
        Task<RoomsSummaryDto> GetRoomStatusSummaryAsync();
        Task AddAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task<bool> RoomNumberExistsAsync(string roomNumber);
        Task<bool> DeleteRoomByIdAsync(int id);
    }
}
