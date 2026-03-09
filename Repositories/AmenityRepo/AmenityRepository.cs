using guest_house_management_backend.Data;

namespace guest_house_management_backend.Repositories.AmenityRepo
{
    public class AmenityRepository:IAmenityRepository
    {
        private readonly DBContext _context;

        public AmenityRepository(DBContext context)
        {
            _context = context;
        }

        //public async Task<List<RoomAmenity>> GetByIdsAsync(List<int> ids)
        //{
        //    return await _context.RoomTypeAmenities
        //        .Where(a => ids.Contains(a.AmenityId))
        //        .ToListAsync();
        //}
    }
}

