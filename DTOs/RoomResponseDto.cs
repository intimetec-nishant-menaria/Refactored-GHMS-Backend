using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class RoomResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomStatusEnum RoomStatus { get; set; }
    }
}
