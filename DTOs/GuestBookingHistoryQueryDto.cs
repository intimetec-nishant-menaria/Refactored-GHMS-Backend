namespace guest_house_management_backend.DTOs
{
    public class GuestBookingHistoryQueryDto
    {
        int pageNumber { get; set; } = 1;
        int pageSize { get; set; } = 10;
    }
}
