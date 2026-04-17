using AutoMapper;
using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Hubs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.BookingRepo;
using guest_house_management_backend.Repositories.RoomRepo;
using guest_house_management_backend.Repositories.UnitOfWorkRepo;
using guest_house_management_backend.Services.Email;
using Microsoft.AspNetCore.SignalR;

namespace guest_house_management_backend.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<BookingHub> _hubContext;

        public BookingService(IHubContext<BookingHub> hubContext,IBookingRepository repository,IRoomRepository roomRepository ,DBContext context ,IEmailSender emailSender, IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _repository = repository;
            _roomRepository = roomRepository;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            return await _repository.GetAvailableRooms(roomAvaiblityRequest);
        }

        public async Task<Paging<BookingResponseDto>> GetAllAsync(int pageNumber , int pageSize , string searchUser , string roomNumber ,int statusFilter)
        {
            return await _repository.GetAllAsync(pageNumber , pageSize , searchUser , roomNumber , statusFilter);
        }


        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if(booking == null)
            {
                return null;
            }
            return _mapper.Map<BookingResponseDto>(booking);
        }
        public async Task CreateAsync(CreateBookingDto createRequest)
        {
            Booking booking;
            await _unitOfWork.BeginTransactionAsync();
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
                var room = await _roomRepository.GetRoomByIdAsync(createRequest.RoomId);

                if (room == null)
                {
                    throw new KeyNotFoundException("Room not found");
                }
                booking = new Booking
                {
                    BugId = createRequest.BugId,
                    RoomId = createRequest.RoomId,
                    GuestName = createRequest.GuestName,
                    GuestEmail =createRequest.GuestEmail,
                    GuestGender = createRequest.Gender,
                    CheckInDate = createRequest.CheckInDate,
                    CheckOutDate = createRequest.CheckOutDate,
                    Status = Enums.BookingStatusEnum.Booked,
                };
                await _repository.AddAsync(booking);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            _ = _emailSender.SendEmailASync(
                createRequest.GuestEmail!,
                "Booking Confirmed - Guest House Management",
                $"""
                <h3>Booking Confirmed!</h3>
                <p>Dear {createRequest.GuestName},</p>
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
            await _hubContext.Clients.All.SendAsync("ReceiveBookingUpdate", _mapper.Map<BookingResponseDto>(booking));
        }

        public async Task UpdateAsync(int id, UpdateBookingDto updateRequest)
        {
            await _unitOfWork.BeginTransactionAsync();

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

                booking.BugId = updateRequest.BugId;
                booking.GuestEmail = updateRequest.GuestEmail;
                booking.GuestName = updateRequest.GuestName;
                booking.RoomId = updateRequest.RoomId;
                booking.GuestGender = updateRequest.Gender;
                booking.CheckInDate = updateRequest.CheckInDate;
                booking.CheckOutDate = updateRequest.CheckOutDate;

                await _repository.UpdateBookingAsync(booking);
                await _unitOfWork.CommitAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveBookingUpdate", _mapper.Map<BookingResponseDto>(booking));
            }
            catch
            {   
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteAsync(int id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            await _repository.DeleteAysnc(booking.Id);
            await _repository.SaveChangesAsync();
        }

        public async Task CancelBooking(int id)
        {
            var booking = await _repository.getBookingById(id);
            if (booking == null)
                throw new KeyNotFoundException("Booking not Found");

            booking.Status = BookingStatusEnum.Cancelled;
            await _repository.UpdateBookingAsync(booking);

            _ = _emailSender.SendEmailASync(
                booking.GuestEmail,
                "Booking Cancelled",
                $"""
                <h3>Booking Cancelled</h3>
                <p>Dear {booking.GuestName},</p>
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

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByRange(DateTime start, DateTime end)
        {
            return await _repository.fetchByRange(start, end);
        }

        public async Task<Paging<BookingResponseDto>> GetUserBookingsAsync(int pageNumber, int pageSize, string guestEmail , string roomNumber , int statusFilter)
        {
            return await _repository.GetUserBookings(pageNumber, pageSize, guestEmail, roomNumber , statusFilter);   
        }
    }
}
