using guest_house_management_backend.DTOs;
using guest_house_management_backend.Repositories;

namespace guest_house_management_backend.Services.RoomType
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _repository;

        public RoomTypeService(IRoomTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RoomTypeResponseDto>> GetAllAsync()
        {
            var roomTypes = await _repository.GetAllAsync();

            return roomTypes.Select(rt => new RoomTypeResponseDto
            {
                Id = rt.Id,
                RoomTypeName = rt.RoomTypeName.ToString(),
                Capacity = rt.Capacity,
                PricePerNight = rt.PricePerNight,
                Amenities = rt.RoomTypeAmenities.Select(a => a.Amenity!.Name).ToList()
            }).ToList();
        }

        public async Task<RoomTypeResponseDto> GetByIdAsync(int id)
        {
            var rt = await _repository.GetByIdAsync(id);

            if (rt == null)
                return null;

            return new RoomTypeResponseDto
            {
                Id = rt.Id,
                RoomTypeName = rt.RoomTypeName.ToString(),
                Capacity = rt.Capacity,
                PricePerNight = rt.PricePerNight,
                Amenities = rt.RoomTypeAmenities
                    .Select(a => a.Amenity!.Name)
                    .ToList()
            };
        }
    }
}
