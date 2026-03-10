using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; } = null!;

        [Required]
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;

        [Required]
        public Enums.RoomStatusEnum RoomStatus { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
