using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;

        [Required]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public string TransactionId { get; set; } = null!;
    }
}
