using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string HashPassword { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsActive { get; set; } = false; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();

    }
}
