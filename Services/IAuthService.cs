using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services
{
    public interface IAuthService
    {
        public Task<string?> LoginAsync(LoginDto dto);
        public Task<bool> RegisterAsync(RegisterDto dto);
    }
}
