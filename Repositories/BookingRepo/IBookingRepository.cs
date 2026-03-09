using guest_house_management_backend.Models;
﻿using guest_house_management_backend.DTOs;

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
    }
}
