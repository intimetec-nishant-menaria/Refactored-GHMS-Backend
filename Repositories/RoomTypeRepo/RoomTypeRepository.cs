//using guest_house_management_backend.Data;
//using guest_house_management_backend.DTOs;
//using guest_house_management_backend.Models;
//using Microsoft.EntityFrameworkCore;

//namespace guest_house_management_backend.Repositories.RoomTypeRepo
//{
//    public class RoomTypeRepository : IRoomTypeRepository
//    {
//        private readonly DBContext _context;

//        public RoomTypeRepository(DBContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<RoomType>> GetAllRoomTypeAsync()
//        {
//            return await _context.RoomTypes
//                .Include(rt => rt.RoomTypeAmenities)
//                .ThenInclude(rta => rta.Amenity)
//                .ToListAsync();
//        }

//        public async Task<RoomType?> GetRoomTypeByIdAsync(int id)
//        {
//            return await _context.RoomTypes
//                .Include(rt => rt.RoomTypeAmenities)
//                .ThenInclude(rta => rta.Amenity) 
//                .FirstOrDefaultAsync(x => x.Id == id);
//        }

//        public async Task DeleteRoomTypeAsync(int id)
//        {
//            var roomType = await _context.RoomTypes.FindAsync(id);

//            if (roomType == null)
//            {
//                return;
//            }
//            _context.RoomTypes.Remove(roomType);
//            await _context.SaveChangesAsync();
//        }

//        public async Task AddRoomTypeAsync(RoomType roomType)
//        {
//            await _context.RoomTypes.AddAsync(roomType);
//            await _context.SaveChangesAsync();
//        }

//        public Task UpdateRoomTypeAsync(RoomType roomType)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<IEnumerable<AmenitiesResponseDto>> GetAllAminities()
//        {
//            return await _context.Amenities.Select(a => new AmenitiesResponseDto
//            {
//                Id = a.Id,
//                Name = a.Name,
//            }).ToListAsync();
//        }
//    }
//}