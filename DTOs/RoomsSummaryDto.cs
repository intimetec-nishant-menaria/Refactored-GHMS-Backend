namespace guest_house_management_backend.DTOs
{
    public class RoomsSummaryDto
    {
        public int Available { get; set; }
        public int Occupied { get; set; }
        public int Maintenance { get; set; }
        public int OutOfOrder { get; set; }
    }
}
