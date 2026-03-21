using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.UserTokenRepo
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly Data.DBContext _context;

        public UserTokenRepository(Data.DBContext context)
        {
            _context = context;
        }
        public async Task AddTokenAsync(UserToken token)
        {

            await _context.UserTokens.AddAsync(token);
        }

        public async Task<UserToken?> GetValidTokenAsync(int userId,string token, UserTokenEnum tokenType)
        {
            return await _context.UserTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t =>
                    t.UserId == userId &&
                    t.Token == token &&
                    t.Type == tokenType &&
                    !t.IsUsed &&
                    t.Expiry > DateTime.UtcNow);
        }

        public async Task UpdateTokenAsync(UserToken token)
        {
            _context.UserTokens.Update(token);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
