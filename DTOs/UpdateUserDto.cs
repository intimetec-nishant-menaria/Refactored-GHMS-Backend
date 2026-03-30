using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Active status is required")]
        public bool IsActive { get; set; }
    }
}
