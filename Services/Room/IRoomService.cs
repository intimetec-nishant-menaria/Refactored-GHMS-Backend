using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Enums;

namespace guest_house_management_backend.Services.Room
{
    public interface IRoomService
    {
        Task<Paging<RoomResponseDto>> getAllRoomAsync(int pageNumber , int pageSize , int roomStatus , string roomNumber);  
        Task<bool> UpdateRoomStatusAsync(int id, RoomStatusEnum status);
        Task<RoomStatusEnum?> GetRoomStatusAsync(int id);
        Task<RoomsSummaryDto> GetRoomStatusSummaryAsync();
        Task<bool> CreateRoomAsync(CreateRoomDto roomRequest);
        Task<bool> UpdateRoomASync(int roomId , UpdateRoomDto updateRoomRequest);
        Task<bool> DeleteRoomByIdASync(int id);
    }
}
