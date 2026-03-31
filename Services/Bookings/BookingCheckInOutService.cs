using AutoMapper;
using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Hubs;
using guest_house_management_backend.Repositories.BookingRepo;
using Microsoft.AspNetCore.SignalR;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingCheckInOutService : IBookingCheckInOutService
    {
        private readonly DBContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IHubContext<BookingHub> _hubContext;
        private readonly IMapper _mapper;

        public BookingCheckInOutService(IHubContext<BookingHub> hubContext,IMapper mapper,DBContext context, IBookingRepository bookingRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }
        public async Task<CheckInResponseDto> CheckInAsync(int bookingID)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingID);
                if (booking == null)
                {
                    throw new KeyNotFoundException("Booking not found!");
                }

                if (booking.Status != BookingStatusEnum.Booked)
                {
                    throw new Exception("Only booked reservations can be checked in!");
                }   

                if (booking.Room.Status != RoomStatusEnum.Available)
                {
                    throw new Exception("Room is not available!");
                }

                booking.Status = BookingStatusEnum.CheckedIn;
                booking.CheckInTime = DateTime.UtcNow;
                booking.Room.Status = RoomStatusEnum.Occupied;

                await _bookingRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveBookingUpdate", _mapper.Map<BookingResponseDto>(booking));

                return new CheckInResponseDto
                {
                    BookingId = booking.Id,
                    GuestName = booking.GuestName,
                    RoomNumber = booking.Room.RoomNumber,
                    ActualCheckInTime = booking.CheckInTime,
                    BookingStatus = booking.Status.ToString(),
                    RoomStatus = booking.Room.Status.ToString()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<CheckOutResponseDto> CheckOutAsync(int bookingID)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingID);
                if (booking == null)
                {
                    throw new KeyNotFoundException("Booking not found!");
                }

                if (booking.Status != BookingStatusEnum.CheckedIn)
                {
                    throw new Exception("Only checked-in bookings can be checked out!");
                }

                var actualCheckOutTime = DateTime.UtcNow;


                booking.Status = BookingStatusEnum.Completed;
                booking.CheckOutTime = actualCheckOutTime;

                booking.Room.Status = RoomStatusEnum.Available;

                await _bookingRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveBookingUpdate", _mapper.Map<BookingResponseDto>(booking));

                return new CheckOutResponseDto
                {
                    BookingId = booking.Id,
                    GuestName = booking.GuestName,
                    RoomNumber = booking.Room.RoomNumber,
                    CheckInTime = booking.CheckInTime,
                    CheckOutTime = actualCheckOutTime,
                    BookingStatus = booking.Status.ToString(),
                    RoomStatus = booking.Room.Status.ToString()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
