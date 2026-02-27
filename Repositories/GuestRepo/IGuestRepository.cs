using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.GuestRepo
{
    public interface IGuestRepository
    {
        public Task<List<Guest>> GetAllAsync();
        public Task<Guest?> GetByIdAsync(int guestId);
        public Task<List<Guest>> SearchAsync(string search);
        public Task<bool> IsDuplicateAsync(string email, string contact);
        public Task AddAsync(Guest guest);
        public Task UpdateAsync(Guest guest);
        public Task DeleteAsync(Guest guest);
    }
}
