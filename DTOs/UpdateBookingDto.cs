using guest_house_management_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class UpdateBookingDto
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatusEnum Status { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
