using guest_house_management_backend.DTOs;
﻿using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public interface IBookingRepository
    {
        Task<List<BookingResponseDto>> GetAllAsync();
        Task<BookingResponseDto?> GetByIdAsync(int id);
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

        public Task<IEnumerable<CalendarEventResponceDto>> fetchByRange(DateTime start, DateTime end);

        public Task<Booking?> getBookingById(int id);
    }
}
