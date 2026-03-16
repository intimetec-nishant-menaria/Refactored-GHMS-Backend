using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace guest_house_management_backend.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        [Required]
        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutTime { get; set; }
        [Required]
        public BookingStatusEnum Status { get; set; }
        public decimal price { get; set; }
        public decimal? FinalBillAmount { get; set; }
        public bool IsPaymentCompleted { get; set; } = false;
        public string? SpecialRequests { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
