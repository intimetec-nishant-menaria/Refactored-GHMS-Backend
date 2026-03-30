using guest_house_management_backend.Enums;

namespace guest_house_management_backend.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public GenderEnum GuestGender { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public BookingStatusEnum Status { get; set; } = BookingStatusEnum.Booked;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
