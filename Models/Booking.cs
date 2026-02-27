using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace guest_house_management_backend.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        [Required]
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [Required]
        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public BookingStatusEnum Status { get; set; } = BookingStatusEnum.Pending;
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
