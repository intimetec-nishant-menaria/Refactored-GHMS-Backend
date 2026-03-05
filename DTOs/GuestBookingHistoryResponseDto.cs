namespace guest_house_management_backend.DTOs
{
    public class GuestBookingHistoryResponseDto
    {
        public List<BookingHistoryDto> Bookings { get; set; } = new();
        public GuestStaticsDto Statistics { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string? PreferredRoomType { get; set; }
        public int? PreferredFloor { get; set; }
    }
}