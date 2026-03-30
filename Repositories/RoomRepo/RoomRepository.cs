using AutoMapper;
using AutoMapper.QueryableExtensions;
using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.RoomRepo
{
    public class RoomRepository: IRoomRepository
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public RoomRepository(DBContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            try
            {
                return await _context.Rooms.FirstOrDefaultAsync(r=> r.Id == id);
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
                if (room.Status != status)
                {
                    room.Status = status;
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

        public async Task<RoomsSummaryDto> GetRoomStatusSummaryAsync()
        {
            try
            {
                return new RoomsSummaryDto
                {
                    Available = await _context.Rooms.CountAsync(r => r.Status == Enums.RoomStatusEnum.Available),
                    Occupied = await _context.Rooms.CountAsync(r => r.Status == Enums.RoomStatusEnum.Occupied),
                    Maintenance = await _context.Rooms.CountAsync(r => r.Status == Enums.RoomStatusEnum.Maintenance),
                    OutOfOrder = await _context.Rooms.CountAsync(r => r.Status == Enums.RoomStatusEnum.OutOfOrder)
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

        public async Task<Paging<RoomResponseDto>> GetAllRoomsAsync(int pageNumber , int pageSize , int roomStatus , string roomNumber)
        {
            var query = _context.Rooms.AsQueryable();

            if (roomStatus != 0)
            {
                query = query.Where(r => r.Status == (Enums.RoomStatusEnum)roomStatus);
            }

            if (!string.IsNullOrEmpty(roomNumber))
            {
                query = query.Where(r => r.RoomNumber.Contains(roomNumber));
            }

            var totalCount = await query.CountAsync();
            var res = await query.OrderBy(r=>r.RoomNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<RoomResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new Paging<RoomResponseDto>
            {
                Data = res,
                MetaData =
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber
                }
            };
        }
    }
}
