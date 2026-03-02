namespace guest_house_management_backend.DTOs
{
    public class CheckOutResponseDto
    {
        public int BookingId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public int TotalNights { get; set; }
        public decimal FinalBillAmount { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public string RoomStatus { get; set; } = string.Empty;
    }
}
