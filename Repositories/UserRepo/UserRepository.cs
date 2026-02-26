using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
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

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            return await _context.Users
             .Select(u => new UserResponseDto
             {
                 Id = u.Id,
                 Name = u.Name,
                 Email = u.Email,
                 Role = u.Role.RoleName,
                 IsActive = u.IsActive,
                 IsEmailConfirmed = u.IsEmailConfirmed,
                 CreatedAt = u.CreatedAt
             })
             .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
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
