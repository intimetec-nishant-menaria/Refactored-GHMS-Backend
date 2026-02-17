using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.UserTokenRepo
{
    public interface IUserTokenRepository
    {
        Task AddTokenAsync(UserToken token);
        Task<UserToken?> GetValidTokenAsync(int userId ,string token, TokenType tokenType);
        Task UpdateTokenAsync(UserToken token);
        public Task SaveChangesAsync();
    }
}
