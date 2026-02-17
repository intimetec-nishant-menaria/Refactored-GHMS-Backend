using System.ComponentModel.DataAnnotations;
namespace guest_house_management_backend.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } =string.Empty;
    }
}
