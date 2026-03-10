using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.Bookings
{
    public interface IBookingService
    {
        Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest);
        Task <IEnumerable<BookingResponseDto>> GetAllAsync();
        Task<guest_house_management_backend.Models.Booking?> GetByIdAsync(int id);
        Task CreateAsync(CreateBookingDto dto);
        Task<guest_house_management_backend.Models.Booking> UpdateAsync(int id, UpdateBookingDto dto);
        Task DeleteAsync(int id);
        Task CancelBooking(int id);
        Task<IEnumerable<CalendarEventResponceDto>> GetBookingsByRange(DateTime start , DateTime end);
    }
}
