using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.AvailableRoomRepo;
using guest_house_management_backend.Repositories.BookingRepo;
using guest_house_management_backend.Repositories.RoomRepo;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IAvailRoomRepository _availRoomRepository;
        private readonly DBContext _context;

        public BookingService(IBookingRepository repository,IAvailRoomRepository availRoomRepository ,DBContext context)
        {
            _repository = repository;
            _availRoomRepository = availRoomRepository;
            _context = context;
        }

        public async Task<List<BookingResponseDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Booking> CreateAsync(CreateBookingDto createRequest)
        {
            ValidateDates(createRequest.CheckInDate, createRequest.CheckOutDate);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var availableRooms = await _availRoomRepository.GetAvailableRoomAsync(
                    createRequest.CheckInDate,
                    createRequest.CheckOutDate);

                var room = availableRooms.FirstOrDefault(r => r.Id == createRequest.RoomId);

                if (room == null)
                {
                    throw new InvalidOperationException("Room not available.");
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

        public async Task<BookingResponseDto> UpdateAsync(int id, UpdateBookingDto updateRequest)
        {
            ValidateDates(updateRequest.CheckInDate, updateRequest.CheckOutDate);

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _repository.GetBookingWithDetailsAsync(id);

                if (booking == null)
                {
                    throw new KeyNotFoundException("Booking not found.");
                }

                var availableRooms = await _availRoomRepository
                    .GetAvailableRoomAsync(updateRequest.CheckInDate, updateRequest.CheckOutDate);

                var room = availableRooms.FirstOrDefault(r => r.Id == booking.RoomId);

                if (room == null)
                {
                    throw new InvalidOperationException("Room not available for the selected dates.");
                }

                booking.CheckInDate = updateRequest.CheckInDate;
                booking.CheckOutDate = updateRequest.CheckOutDate;
                booking.Status = updateRequest.Status;
                booking.SpecialRequests = updateRequest.SpecialRequests;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new BookingResponseDto
                {
                    Id = booking.Id,
                    GuestId = booking.GuestId,
                    GuestName = booking.Guest.Name,
                    RoomId = booking.RoomId,
                    RoomNumber = booking.Room.RoomNumber,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    Status = booking.Status,
                    price = booking.price,
                    SpecialRequests = booking.SpecialRequests
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAysnc(id);
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

        private decimal CalculatePrice(Models.Room room, DateTime checkIn, DateTime checkOut)
        {
            var days = (checkOut - checkIn).Days;
            return days * room.RoomType.PricePerNight;
        }
    }
}
