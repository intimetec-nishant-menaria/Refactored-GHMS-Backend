using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is Required")]

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } =string.Empty;

    }
}
