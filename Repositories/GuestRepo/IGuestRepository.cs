using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.GuestRepo
{
    public interface IGuestRepository
    {
        public Task<Paging<GuestResponseDto>> GetAllAsync(int pageNumber ,int pageSize , string searchUser);
        public Task<Guest?> GetByIdAsync(int guestId);
        public Task<Guest?> GetByEmailAsync(string email);
        public Task<List<Guest>> SearchAsync(string search);
        public Task<bool> IsDuplicateAsync(string email, string contact);
        public Task AddAsync(Guest guest);
        public Task UpdateAsync(Guest guest);
        public Task DeleteAsync(Guest guest);
        public Task<List<Booking>> GetGuestBookings(int guestId);
    }
}
