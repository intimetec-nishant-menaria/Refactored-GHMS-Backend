namespace guest_house_management_backend.DTOs
{
    public class BookingHistoryDto
    {
        public int BookingId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
