using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class CalendarEventResponceDto
    {
        public int BookingId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty ;
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public BookingStatusEnum BookingStatus { get; set; }

    }
}
