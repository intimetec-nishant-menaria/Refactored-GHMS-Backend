using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.DTOs
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public string GuestEmail { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
