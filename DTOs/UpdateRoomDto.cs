using guest_house_management_backend.Enums;
using Microsoft.Identity.Client;

namespace guest_house_management_backend.DTOs
{
    public class UpdateRoomDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int RoomTypeId { get; set; }
        public RoomStatusEnum RoomStatus { get; set; }
    }
}
