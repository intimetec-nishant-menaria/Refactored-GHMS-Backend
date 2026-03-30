using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string HashPassword { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    }
}
