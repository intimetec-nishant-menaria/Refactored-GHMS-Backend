using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.UserRepo
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetByIdAsync(int id);
        public Task AddUserAsync(User user);
        public Task DeleteAsync(int id);
        public Task UpdateAsync(User user);
        public Task SaveChangesAsync();
    }
}
