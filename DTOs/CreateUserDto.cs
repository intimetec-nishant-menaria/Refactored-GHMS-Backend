using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;
namespace guest_house_management_backend.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email {  get; set; } = string.Empty ;
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Role is required")]
        public RoleEnum Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
