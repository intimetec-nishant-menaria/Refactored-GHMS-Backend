using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.BookingRepo;
using guest_house_management_backend.Repositories.RoomRepo;
using guest_house_management_backend.Services.Email;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IRoomRepository _roomRepository;
        private readonly DBContext _context;
        private readonly IEmailSender _emailSender;

        public BookingService(IBookingRepository repository,IRoomRepository roomRepository ,DBContext context ,IEmailSender emailSender)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            return await _repository.GetAvailableRooms(roomAvaiblityRequest);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(CreateBookingDto createRequest)
        {
            //ValidateDates(createRequest.CheckInDate, createRequest.CheckOutDate);

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
                var room = await _context.Rooms
                            .Include(r => r.RoomType)
                            .FirstOrDefaultAsync(r => r.Id == createRequest.RoomId);
                if(room == null)
                {
                    throw new KeyNotFoundException("Room not found");
                }
                var totalPrice = CalculatePrice(room, createRequest.CheckInDate, createRequest.CheckOutDate);
                var booking = new Booking
                {
                    UserId = createRequest.UserId,
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
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Booking> UpdateAsync(int id, UpdateBookingDto updateRequest)
        {
            //ValidateDates(updateRequest.CheckInDate, updateRequest.CheckOutDate);

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

                if(booking.Status == BookingStatusEnum.CheckedIn)
                {
                   await  _roomRepository.UpdateRoomStatusAsync(booking.RoomId, Enums.RoomStatusEnum.Occupied);
                }else if(booking.Status == BookingStatusEnum.Completed)
                {
                    await _roomRepository.UpdateRoomStatusAsync(booking.RoomId, Enums.RoomStatusEnum.Available);
                }

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

        public async Task CancelBooking(int id)
        {
            var booking = await GetByIdAsync(id);
            if (booking == null)
                throw new KeyNotFoundException("Booking not Found");

            booking.Status = BookingStatusEnum.Cancelled;
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(booking.User.Id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            await _emailSender.SendEmailASync(
                user.Email,
                "Booking Cancelled",
                $"""
                <h3>Booking Cancelled</h3>
                <p>Dear {user.Name},</p>
                <p>Your booking with ID <strong>{booking.Id}</strong> has been cancelled successfully.</p>
                <p>Booking Details:</p>
                <ul>
                    <li>Room Number: {booking.Room.RoomNumber}</li>
                    <li>Check-In Date: {booking.CheckInDate:MMMM dd, yyyy}</li>
                    <li>Check-Out Date: {booking.CheckOutDate:MMMM dd, yyyy}</li>
                    <li>Status: Cancelled</li>
                </ul>
                <p>If you did not request this cancellation, please contact our support team immediately.</p>
                <p>Thank you for using our services.</p>
                """
            );
        }

        public async Task<IEnumerable<CalendarEventResponceDto>> GetBookingsByRange(DateTime start, DateTime end)
        {
            return await _repository.fetchByRange(start, end);
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
