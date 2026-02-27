namespace guest_house_management_backend.DTOs
{
    public class RoomTypeResponseDto
    {
        public int Id { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public List<string> Amenities { get; set; } = new();
    }
}
