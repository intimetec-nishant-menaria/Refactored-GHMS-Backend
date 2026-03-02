namespace guest_house_management_backend.DTOs
{
    public class CheckInResponseDto
    {
        public int BookingId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime ActualCheckInTime { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public string RoomStatus { get; set; } = string.Empty;
    }
}
