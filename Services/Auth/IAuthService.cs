using guest_house_management_backend.DTOs;

namespace guest_house_management_backend.Services.Auth
{
    public interface IAuthService
    {
        public Task<string?> LoginAsync(LoginDto dto);
        public Task<bool> RegisterAsync(RegisterDto dto);
        public Task<(bool Success , string Message)> ChangePassword(int UserId,ChangePasswordDto dto);
        public Task ForgetPasswordAsync(ForgetPasswordDto forgetPassword);
        Task<bool> VerifyResetTokenAsync(string email, string token);
        public Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
