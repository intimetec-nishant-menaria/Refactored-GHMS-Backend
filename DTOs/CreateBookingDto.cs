using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class CreateBookingDto
    {
        public int BugId { get; set; }
        public int RoomId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public GenderEnum Gender { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
