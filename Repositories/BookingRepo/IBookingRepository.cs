
using guest_house_management_backend.DTOs;
﻿using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public interface IBookingRepository
    {
        public Task<IEnumerable<RoomResponseDto>> GetAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest);
        Task<IEnumerable<BookingResponseDto>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAysnc(Booking booking);
        Task<bool> IsRoomAvailableAsync(
            int roomId,
            DateTime CheckIn,
            DateTime CheckOut,
            int? excludeBookingId = null);

        public Task<IEnumerable<CalendarEventResponceDto>> fetchByRange(DateTime start, DateTime end);
    }
}
