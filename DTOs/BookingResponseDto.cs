using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public GenderEnum Gender { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatusEnum Status { get; set; }
    }
}
