using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatusEnum Status { get; set; }
        public decimal? price { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
