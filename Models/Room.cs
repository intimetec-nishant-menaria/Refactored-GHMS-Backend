using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Guid RoomStatusId { get; set; }
        public RoomStatus RoomStatus { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
