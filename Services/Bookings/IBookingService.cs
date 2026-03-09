using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.Bookings
{
    public interface IBookingService
    {
        Task<List<BookingResponseDto>> GetAllAsync();
        Task<BookingResponseDto?> GetByIdAsync(int id);
        Task<guest_house_management_backend.Models.Booking> CreateAsync(CreateBookingDto createRequest);
        Task<BookingResponseDto> UpdateAsync(int id, UpdateBookingDto updateRequest);
        Task DeleteAsync(int id);
    }
}
