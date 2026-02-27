using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class CreateRoomTypeDto
    {
        public RoomTypeEnum RoomTypeName { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public List<int> AmenityIds { get; set; } = new();
    }
}
