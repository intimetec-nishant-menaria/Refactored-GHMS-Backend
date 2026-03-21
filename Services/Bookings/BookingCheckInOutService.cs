using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Repositories.BookingRepo;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingCheckInOutService : IBookingCheckInOutService
    {
        private readonly DBContext _context;
        private readonly IBookingRepository _bookingRepository;
        public BookingCheckInOutService(DBContext context, IBookingRepository bookingRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
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

                if (booking.Room.RoomStatus != RoomStatusEnum.Available)
                {
                    throw new Exception("Room is not available!");
                }

                booking.Status = BookingStatusEnum.CheckedIn;
                booking.CheckInTime = DateTime.UtcNow;
                booking.Room.RoomStatus = RoomStatusEnum.Occupied;

                await _bookingRepository.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CheckInResponseDto
                {
                    BookingId = booking.Id,
                    GuestName = booking.Guest.Name,
                    RoomNumber = booking.Room.RoomNumber,
                    ActualCheckInTime = booking.CheckInTime.Value,
                    BookingStatus = booking.Status.ToString(),
                    RoomStatus = booking.Room.RoomStatus.ToString()
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

                var totalNights = (actualCheckOutTime.Date - booking.CheckInTime!.Value.Date).Days;
                if (totalNights <= 0)
                {
                    totalNights = 1;
                }

                var finalBill = totalNights * booking.Room.RoomType.PricePerNight;

                booking.Status = BookingStatusEnum.Completed;
                booking.CheckOutTime = actualCheckOutTime;
                booking.FinalBillAmount = finalBill;
                booking.IsPaymentCompleted = true;

                booking.Room.RoomStatus = RoomStatusEnum.Available;

                await _bookingRepository.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CheckOutResponseDto
                {
                    BookingId = booking.Id,
                    GuestName = booking.Guest.Name,
                    RoomNumber = booking.Room.RoomNumber,
                    CheckInTime = booking.CheckInTime.Value,
                    CheckOutTime = actualCheckOutTime,
                    TotalNights = totalNights,
                    FinalBillAmount = finalBill,
                    BookingStatus = booking.Status.ToString(),
                    RoomStatus = booking.Room.RoomStatus.ToString()
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
