using AutoMapper;
using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.AvailableRoomRepo
{
    public class AvailRoomRepostiory : IAvailRoomRepository
    {
        private readonly DBContext _context;

        public AvailRoomRepostiory(DBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomAsync(GenderEnum gender,DateTime checkIn, DateTime checkOut)
        {
            var query = _context.Rooms.AsQueryable();
            if (gender == GenderEnum.Female)
            {
                query = query.Where(r => r.Floor == 1 || r.Floor == 3 || r.Floor == 4);
            }
            else if (gender == GenderEnum.Male)
            {
                query = query.Where(r => r.Floor == 2 || r.Floor == 3 || r.Floor == 4);
            }

            query = query.Where(r => r.Status != RoomStatusEnum.Maintenance && r.Status != RoomStatusEnum.OutOfOrder);

            return await query
                .Select(r => new RoomResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    Floor = r.Floor,
                    Status = r.Status,
                    CurrentOccupancy = r.Bookings
                        .Count(b =>
                            b.Status != BookingStatusEnum.Cancelled &&
                            b.Status != BookingStatusEnum.Completed &&
                            b.CheckInDate < checkOut &&
                            b.CheckOutDate > checkIn
                        )
                })
                .Where(dto => dto.CurrentOccupancy < 2)
                .OrderByDescending(r => r.CurrentOccupancy)
                .ThenBy(r => r.Floor)
                .ThenBy(r => r.RoomNumber)
                .ToListAsync();
        }
    }
}
