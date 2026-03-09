using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.RoomRepo;
using guest_house_management_backend.Repositories.RoomTypeRepo;

namespace guest_house_management_backend.Services.Room
{
    public class RoomService: IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomService(IRoomRepository roomRepository , IRoomTypeRepository roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _roomTypeRepository = roomTypeRepository;
            
        }

        public async Task<bool> UpdateRoomStatusAsync(int id, Enums.RoomStatusEnum status)
        {
            return await _roomRepository.UpdateRoomStatusAsync(id, status);
        }

        public async Task<Enums.RoomStatusEnum?> GetRoomStatusAsync(int id)
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

        public async Task<bool> CreateRoomAsync(CreateRoomDto roomRequest)
        {
            if (roomRequest == null)
                throw new ArgumentNullException(nameof(roomRequest));

            if (await _roomRepository.RoomNumberExistsAsync(roomRequest.RoomNumber))
                throw new Exception("Room number already exists.");

            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(roomRequest.RoomTypeId);
            if (roomType == null)
                throw new Exception("Invalid Room Type.");
            var room = new Models.Room
            {
                RoomNumber = roomRequest.RoomNumber,
                RoomTypeId = roomRequest.RoomTypeId,
                RoomStatus = Enums.RoomStatusEnum.Available,
                CreatedAt = DateTime.UtcNow
            };
            await _roomRepository.AddAsync(room);
            return true;
        }

        public async Task<bool> DeleteRoomByIdASync(int id)
        {
            return await _roomRepository.DeleteRoomByIdAsync(id);
        }

        public async Task<bool> UpdateRoomASync(int roomId,UpdateRoomDto updateRoomRequest)
        {

            if(updateRoomRequest == null)
                throw new ArgumentNullException(nameof(updateRoomRequest));

            var existingRoom = await _roomRepository.GetRoomByIdAsync(roomId);
            if(existingRoom == null)
                throw new Exception("Room not found.");

            if(existingRoom.RoomNumber != updateRoomRequest.RoomNumber)
            {
                bool roomNumberExists = await _roomRepository.RoomNumberExistsAsync(updateRoomRequest.RoomNumber);

                if (roomNumberExists)
                    throw new Exception("Room number already exists.");
            }

            existingRoom.RoomNumber = updateRoomRequest.RoomNumber;
            existingRoom.RoomTypeId = updateRoomRequest.RoomTypeId;
            existingRoom.RoomStatus = updateRoomRequest.RoomStatus;
            existingRoom.UpdatedAt = DateTime.UtcNow;

            await _roomRepository.UpdateRoomAsync(existingRoom);

            return true;
        }

        public async Task<IEnumerable<RoomResponseDto>> getAllRoomAsync()
        {
            return await _roomRepository.GetAllRoomsAsync();
        }
    }
}
