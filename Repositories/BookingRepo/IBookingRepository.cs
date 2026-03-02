using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAysnc(Booking booking);
        Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut,
            int? excludeBookingId = null);
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
        Task SaveChangesAsync();
    }
}
