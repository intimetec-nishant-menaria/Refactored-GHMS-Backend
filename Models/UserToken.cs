using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class UserToken
    {
        public Guid Id {  get; set; } = Guid.NewGuid();
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public UserTokenEnum Type  { get; set; }
        [Required]
        public DateTime Expiry {  get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
