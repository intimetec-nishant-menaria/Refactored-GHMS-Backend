using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        [Required]
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [Required]
        public string Action { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string ChangedValue { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
