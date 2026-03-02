using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.AvailableRoomRepo
{
    public interface IAvailRoomRepository
    {
        Task<List<Room>> GetAvailableRoomAsync(DateTime checkIn, DateTime checkOut);
    }
}
