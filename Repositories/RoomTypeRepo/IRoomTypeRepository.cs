using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.RoomTypeRepo
{
    public interface IRoomTypeRepository
    {
        Task<List<RoomType>> GetAllRoomTypeAsync();
        Task<RoomType?> GetRoomTypeByIdAsync(int id);
        Task AddRoomTypeAsync(RoomType roomType);
        Task UpdateRoomTypeAsync(RoomType roomType);
        Task DeleteRoomTypeAsync(int id);
    }
}
