using guest_house_management_backend.Enums;
using guest_house_management_backend.Repositories.RoomRepo;

namespace guest_house_management_backend.Services.Room
{
    public class RoomService: IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<bool> UpdateRoomStatusAsync(int id, RoomStatusEnum status)
        {
            return await _roomRepository.UpdateRoomStatusAsync(id, status);
        }

        public async Task<RoomStatusEnum?> GetRoomStatusAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);

            if (room == null)
                return null;

            return room.RoomStatus;
        }

        public async Task<object> GetRoomStatusSummaryAsync()
        {
            return await _roomRepository.GetRoomStatusSummaryAsync();
        }
    }
}
