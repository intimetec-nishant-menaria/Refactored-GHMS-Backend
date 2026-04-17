using guest_house_management_backend.Enums;

namespace guest_house_management_backend.DTOs
{
    public class UpdateRoomDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public RoomStatusEnum Status { get; set; }
    }
}
