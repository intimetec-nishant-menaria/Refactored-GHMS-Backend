using guest_house_management_backend.Data;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.RoomTypeRepo
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly DBContext _context;

        public RoomTypeRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<List<RoomType>> GetAllAsync()
        {
            return await _context.RoomTypes
                .Include(rt => rt.RoomTypeAmenities)
                .ThenInclude(rta => rta.Amenity)
                .ToListAsync();
        }

        public async Task<RoomType?> GetByIdAsync(int id)
        {
            return await _context.RoomTypes
                .Include(rt => rt.RoomTypeAmenities)
                    .ThenInclude(rta => rta.Amenity) 
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
