using System.ComponentModel.DataAnnotations.Schema;

namespace guest_house_management_backend.DTOs
{
    public class ChangePasswordDto
    {
        [NotMapped]
        public string OldPassword { get; set; } = string.Empty;
        [NotMapped]
        public string NewPassword { get; set; } = string.Empty;
    }
}
