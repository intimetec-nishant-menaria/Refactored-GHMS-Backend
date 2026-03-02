using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Repositories.BookingRepo
{
    public interface IBookingRepository
    {
        public Task<IEnumerable<RoomResponseDto>> GetAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest);
    }
}
