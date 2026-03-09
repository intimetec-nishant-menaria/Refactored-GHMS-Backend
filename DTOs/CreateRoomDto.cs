namespace guest_house_management_backend.DTOs
{
    public class CreateRoomDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public int RoomTypeId { get; set; }
    }
}
