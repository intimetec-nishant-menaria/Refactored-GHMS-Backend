using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
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

        public async Task<bool> UpdateRoomStatusAsync(int id, Enums.RoomStatusEnum status)
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
                    Available = await _context.Rooms.CountAsync(r => r.RoomStatus == Enums.RoomStatusEnum.Available),
                    Occupied = await _context.Rooms.CountAsync(r => r.RoomStatus == Enums.RoomStatusEnum.Occupied),
                    Maintenance = await _context.Rooms.CountAsync(r => r.RoomStatus == Enums.RoomStatusEnum.Maintenance),
                    OutOfOrder = await _context.Rooms.CountAsync(r => r.RoomStatus == Enums.RoomStatusEnum.OutOfOrder)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching room status summary", ex);
            }
        }
        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RoomNumberExistsAsync(string roomNumber)
        {
            return await _context.Rooms
                .AnyAsync(r => r.RoomNumber == roomNumber);
        }

        public async Task<RoomType?> GetRoomTypeByIdAsync(int roomTypeId)
        {
            return await _context.RoomTypes
                .Include(rt => rt.RoomTypeAmenities)
                    .ThenInclude(rta => rta.Amenity)
                .FirstOrDefaultAsync(rt => rt.Id == roomTypeId);
        }

        public async Task<bool> DeleteRoomByIdAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if(room == null)
                return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllRoomsAsync()
        {
            return await _context.Rooms.Select(room => new RoomResponseDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                RoomTypeName = room.RoomType.RoomTypeName.ToString(),
                Capacity = room.RoomType.Capacity,
                PricePerNight = room.RoomType.PricePerNight
            }).ToListAsync();
        }
    }
}
