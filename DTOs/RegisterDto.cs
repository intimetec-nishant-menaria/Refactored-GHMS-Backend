using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string FullName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is Required")]

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        public string Password {  get; set; } = string.Empty;

    }
}
