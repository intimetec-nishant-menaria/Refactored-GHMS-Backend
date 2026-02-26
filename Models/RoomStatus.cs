using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class RoomStatus
    {
        public Guid Id { get; set; }

        [Required]
        public RoomStatusEnum Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
