using guest_house_management_backend.Data;
using guest_house_management_backend.Data.Configurations;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.AvailableRoomRepo;
using guest_house_management_backend.Repositories.BookingRepo;
using guest_house_management_backend.Repositories.GuestRepo;
using guest_house_management_backend.Repositories.RoomRepo;
using guest_house_management_backend.Services.Email;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IRoomRepository _roomRepository;
        private readonly DBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IGuestRepository _guestRepositroy;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(IHttpContextAccessor httpContextAccessor,IGuestRepository guestRepository,IBookingRepository repository,IRoomRepository roomRepository ,DBContext context ,IEmailSender emailSender)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _context = context;
            _emailSender = emailSender;
            _guestRepositroy = guestRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            return await _repository.GetAvailableRooms(roomAvaiblityRequest);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }


        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task CreateAsync(CreateBookingDto createRequest)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var emailFromJwt = user?.FindFirst(ClaimTypes.Email)?.Value;
            var nameFromJwt = user?.FindFirst(ClaimTypes.Name)?.Value;

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
                var guest = await _guestRepositroy.GetByEmailAsync(createRequest.GuestEmail);
                if (guest == null)
                {
                    guest = new guest_house_management_backend.Models.Guest
                    {
                        Name = nameFromJwt,
                        Email = emailFromJwt,
                        CreatedAt = DateTime.UtcNow,
                    };
                    await _guestRepositroy.AddAsync(guest);
                }
                var booking = new Booking
                {
                    GuestId = guest.Id,
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

                await _emailSender.SendEmailASync(
                guest.Email!,
                "Booking Confirmed - Guest House Management",
                $"""
                <h3>Booking Confirmed!</h3>
                <p>Dear {guest.Name},</p>
                <p>We are excited to inform you that your booking with ID <strong>{booking.Id}</strong> has been confirmed.</p>
                <p><strong>Booking Details:</strong></p>
                <ul>
                    <li><strong>Room Number:</strong> {booking.Room.RoomNumber}</li>
                    <li><strong>Check-In Date:</strong> {booking.CheckInDate:MMMM dd, yyyy}</li>
                    <li><strong>Check-Out Date:</strong> {booking.CheckOutDate:MMMM dd, yyyy}</li>
                    <li><strong>Status:</strong> Confirmed</li>
                </ul>
                <p><strong>Arrival Information:</strong></p>
                <p>Please have your ID proof ready at the time of check-in. Our standard check-in time is 12:00 PM.</p>
                <p>If you have any questions or need to modify your stay, please contact us.</p>
                <p>We look forward to hosting you!</p>
                <p>Best Regards,<br/>Management Team</p>
                """);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<BookingResponseDto> UpdateAsync(int id, UpdateBookingDto updateRequest)
        {
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

                return new BookingResponseDto
                {
                    Id = booking.Id,
                    GuestId = booking.GuestId,
                    GuestName = booking.GuestName,
                    RoomNumber = booking.RoomNumber,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    Status = booking.Status,
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
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            await _repository.DeleteAysnc(booking.Id);
            await _context.SaveChangesAsync();
        }

        public async Task CancelBooking(int id)
        {
            var booking = await _repository.getBookingById(id);
            if (booking == null)
                throw new KeyNotFoundException("Booking not Found");

            booking.Status = BookingStatusEnum.Cancelled;
            await _repository.UpdateBookingAsync(booking);

            var guest = await _guestRepositroy.GetByIdAsync(booking.GuestId);
            if (guest == null)
                throw new KeyNotFoundException("User not found");

            await _emailSender.SendEmailASync(
                guest.Email,
                "Booking Cancelled",
                $"""
                <h3>Booking Cancelled</h3>
                <p>Dear {guest.Name},</p>
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

        private decimal CalculatePrice(Models.Room room, DateTime checkIn, DateTime checkOut)
        {
            var days = (checkOut - checkIn).Days;
            return days * room.RoomType.PricePerNight;
        }
    }
}
