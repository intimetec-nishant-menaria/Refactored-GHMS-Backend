using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class GuestResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IDProof { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
