using guest_house_management_backend.Data;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.RoomRepo
{
    public class RoomRepository: IRoomRepository
    {
        private readonly DBContext _context;

        public RoomRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            try
            {
                return await _context.Rooms.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching room by id", ex);
            }
        }

        public async Task<bool> UpdateRoomStatusAsync(int id, RoomStatusEnum status)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);

                if (room == null)
                    return false;
                if (room.RoomStatus != status)
                {
                    room.RoomStatus = status;
                    room.UpdatedAt = DateTime.UtcNow; 
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating room status", ex);
            }
        }

        public async Task<object> GetRoomStatusSummaryAsync()
        {
            try
            {
                return new
                {
                    Available = await _context.Rooms.CountAsync(r => r.RoomStatus == RoomStatusEnum.Available),
                    Occupied = await _context.Rooms.CountAsync(r => r.RoomStatus == RoomStatusEnum.Occupied),
                    Maintenance = await _context.Rooms.CountAsync(r => r.RoomStatus == RoomStatusEnum.Maintenance),
                    OutOfOrder = await _context.Rooms.CountAsync(r => r.RoomStatus == RoomStatusEnum.OutOfOrder)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching room status summary", ex);
            }
        }

    }
}
