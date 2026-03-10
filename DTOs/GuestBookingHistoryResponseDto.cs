namespace guest_house_management_backend.DTOs
{
    public class GuestBookingHistoryResponseDto
    {
        public int GuestId { get; set; }
        public int NumberOfVisits { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalSpent { get; set; }
        public string? PreferredRoomType { get; set; }
        public int? PreferredFloor { get; set; }
        public int TotalRecords { get; set; }
        public List<BookingHistoryDto> Bookings { get; set; } = new();
    }
}