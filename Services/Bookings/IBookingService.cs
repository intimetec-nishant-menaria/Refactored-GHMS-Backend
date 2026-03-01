using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.Bookings
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllAsync();
        Task<guest_house_management_backend.Models.Booking?> GetByIdAsync(int id);
        Task<guest_house_management_backend.Models.Booking> CreateAsync(CreateBookingDto dto);
        Task<guest_house_management_backend.Models.Booking> UpdateAsync(int id, UpdateBookingDto dto);
        Task DeleteAsync(int id);
    }
}
