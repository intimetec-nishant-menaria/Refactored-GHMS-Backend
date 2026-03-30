using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public string EntityName { get; set; } = string.Empty; 

        [Required]
        public string EntityId { get; set; } = string.Empty; 

        [Required]
        public string Action { get; set; } = string.Empty; 

        public string OldValue { get; set; } = string.Empty; 
        public string NewValue { get; set; } = string.Empty; 

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
