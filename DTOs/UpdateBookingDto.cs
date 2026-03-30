using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class UpdateBookingDto
    {
        public int BugId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
