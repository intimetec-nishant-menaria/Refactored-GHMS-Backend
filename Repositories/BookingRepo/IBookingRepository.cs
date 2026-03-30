using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public interface IBookingRepository
    {
        Task<Paging<BookingResponseDto>> GetAllAsync(int pageNumber , int pageSize ,string searchUser , string roomNumber ,int statusFilter);
        Task<Booking> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAysnc(int id);
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
        Task SaveChangesAsync();
        public Task<IEnumerable<RoomResponseDto>> GetAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest);
        Task UpdateBookingAsync(Booking booking);
        Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut,
            int? excludeBookingId = null);

        public Task<IEnumerable<BookingResponseDto>> fetchByRange(DateTime start, DateTime end);
        public Task<Booking?> getBookingById(int id);
        public Task<Paging<BookingResponseDto>> GetUserBookings(int pageNumber, int pageSize, string guestEmail , string roomNumber , int statusFilter);
    }
}
