using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.BookingService
{
    public interface IBookingService
    {
        public Task<IEnumerable<RoomResponseDto>> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest);
    }
}
