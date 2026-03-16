using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.Bookings
{
    public interface IBookingCheckInOutService
    {
        Task<CheckInResponseDto> CheckInAsync(int bookingID);
        Task<CheckOutResponseDto> CheckOutAsync(int bookingID);
    }
}
