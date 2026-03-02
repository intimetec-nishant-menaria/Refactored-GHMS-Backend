using guest_house_management_backend.DTOs;
using guest_house_management_backend.Repositories.BookingRepo;

namespace guest_house_management_backend.Services.BookingService
{
    public class BookingSerivce:IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingSerivce(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            return await _bookingRepository.GetAvailableRooms(roomAvaiblityRequest);
        }
    }
}
