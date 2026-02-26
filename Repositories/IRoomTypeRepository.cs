using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories
{
    public interface IRoomTypeRepository
    {
        Task<List<RoomType>> GetAllAsync();
        Task<RoomType?> GetByIdAsync(int id);
        //Task AddAsync(RoomType roomType);
        //Task UpdateAsync(RoomType roomType);
        //Task DeleteAsync(RoomType roomType);
    }
}
