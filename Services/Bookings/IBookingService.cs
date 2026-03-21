using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.Bookings
{
    public interface IBookingService
    {
        Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest);
        Task <Paging<BookingResponseDto>> GetAllAsync(int pageNumber , int pageSize ,string searchUser , string roomNumber ,int statusFilter);
        Task<BookingResponseDto> GetByIdAsync(int id);
        Task CreateAsync(CreateBookingDto dto);
        Task<BookingResponseDto> UpdateAsync(int id, UpdateBookingDto dto);
        Task DeleteAsync(int id);
        Task CancelBooking(int id);
        Task<IEnumerable<CalendarEventResponceDto>> GetBookingsByRange(DateTime start , DateTime end);
        Task<Paging<BookingResponseDto>> GetUserBookingsAsync(int pageNumber, int pageSize, string guestEmail , string roomNumber , int statusFilter);
    }
}
