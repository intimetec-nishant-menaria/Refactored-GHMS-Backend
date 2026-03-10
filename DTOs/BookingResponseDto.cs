using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatusEnum Status { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
