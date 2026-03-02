using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.BookingRepo;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly DBContext _context;

        public BookingService(IBookingRepository repository, DBContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Booking> CreateAsync(CreateBookingDto createRequest)
        {
            ValidateDates(createRequest.CheckInDate, createRequest.CheckOutDate);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var isAvailable = await _repository.IsRoomAvailableAsync(
                    createRequest.RoomId,
                    createRequest.CheckInDate,
                    createRequest.CheckOutDate);
                if (!isAvailable)
                {
                    throw new InvalidOperationException("Room not available.");
                }
                var room = await _context.Room
                            .Include(r => r.RoomType)
                            .FirstOrDefaultAsync(r => r.Id == createRequest.RoomId);
                if(room == null)
                {
                    throw new KeyNotFoundException("Room not found");
                }
                var totalPrice = CalculatePrice(room, createRequest.CheckInDate, createRequest.CheckOutDate);
                var booking = new Booking
                {
                    GuestId = createRequest.GuestId,
                    RoomId = createRequest.RoomId,
                    CheckInDate = createRequest.CheckInDate,
                    CheckOutDate = createRequest.CheckOutDate,
                    Status = Enums.BookingStatusEnum.Booked,
                    price = totalPrice,
                    SpecialRequests = createRequest.SpecialRequests
                };
                await _repository.AddAsync(booking);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Booking> UpdateAsync(int id, UpdateBookingDto updateRequest)
        {
            ValidateDates(updateRequest.CheckInDate, updateRequest.CheckOutDate);

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _repository.GetByIdAsync(id);

                if (booking == null)
                    throw new KeyNotFoundException("Booking not found.");

                var isAvailable = await _repository.IsRoomAvailableAsync(
                    booking.RoomId,
                    updateRequest.CheckInDate,
                    updateRequest.CheckOutDate,
                    id);

                if (!isAvailable)
                    throw new InvalidOperationException("Room not available for selected dates.");

                booking.CheckInDate = updateRequest.CheckInDate;
                booking.CheckOutDate = updateRequest.CheckOutDate;
                booking.Status = updateRequest.Status;
                booking.SpecialRequests = updateRequest.SpecialRequests;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            await _repository.DeleteAysnc(booking);
            await _context.SaveChangesAsync();
        }

        private void ValidateDates(DateTime checkIn, DateTime checkOut)
        {
            if(checkIn >= checkOut)
            {
                throw new InvalidOperationException("Check-out must be after check-in.");
            }
            if(checkIn < DateTime.Today)
            {
                throw new InvalidOperationException("Check-in date cannot be in the past.");
            }
        }

        private decimal CalculatePrice(Room room, DateTime checkIn, DateTime checkOut)
        {
            var days = (checkOut - checkIn).Days;
            return days * room.RoomType.PricePerNight;
        }
    }
}
