using guest_house_management_backend.Data;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly Data.DBContext _context;

        public UserRepository(Data.DBContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByEmailAsync(string email) 
        {
            return await _context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return;
            }

            _context.Users.Remove(user);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
