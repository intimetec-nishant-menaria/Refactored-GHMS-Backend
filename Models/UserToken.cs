using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace guest_house_management_backend.Models
{
    public enum TokenType
    {
        EmailConfirmation = 1 ,
        ResetPassword = 2
    }
    public class UserToken
    {
        public int Id {  get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public TokenType Type  { get; set; }
        [Required]
        public DateTime Expiry {  get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
