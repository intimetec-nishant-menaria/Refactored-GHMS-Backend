using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class CalendarEventResponceDto
    {
        public int Id { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty ;
        public DateTime CheckInDate {  get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatusEnum Status { get; set; }

    }
}
