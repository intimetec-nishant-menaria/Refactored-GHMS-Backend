using AutoMapper;
using AutoMapper.QueryableExtensions;
using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.AvailableRoomRepo
{
    public class AvailRoomRepostiory : IAvailRoomRepository
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public AvailRoomRepostiory(DBContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RoomResponseDto>> GetAvailableRoomAsync(DateTime checkIn, DateTime checkOut)
        {
            return await _context.Rooms
                .Include(r => r.RoomType)
                .Where(r =>
                    !r.Bookings
                       .Any(b =>
                            b.Status != BookingStatusEnum.Cancelled &&
                            b.Status != BookingStatusEnum.Completed &&
                            (b.CheckOutDate >= checkIn ||
                            b.CheckInDate <= checkOut)
            )
                ).ProjectTo<RoomResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
