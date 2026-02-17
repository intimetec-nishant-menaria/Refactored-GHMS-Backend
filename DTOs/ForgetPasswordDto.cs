using System.ComponentModel.DataAnnotations;
namespace guest_house_management_backend.DTOs
{
    public class ForgetPasswordDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
