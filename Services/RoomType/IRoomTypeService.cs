using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.RoomType
{
    public interface IRoomTypeService
    {
        Task<List<RoomTypeResponseDto>> GetAllAsync();
        Task<RoomTypeResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<AmenitiesResponseDto>> GetAllAminities();
    }
}
