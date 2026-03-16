namespace guest_house_management_backend.DTOs
{
    public class AvailableRoomDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
