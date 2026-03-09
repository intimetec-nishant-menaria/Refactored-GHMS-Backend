using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.Auth
{
    public interface IAuthService
    {
        public Task<(string? token , User user)> LoginUserAsync(LoginDto loginRequest);
        public Task RegisterUserAsync(RegisterDto registerRequest);
        public Task<(bool Success , string Message)> ChangeUserPassword(int UserId,ChangePasswordDto changePasswordRequest);
        public Task ForgetUserPasswordAsync(ForgetPasswordDto forgetPasswordRequest);
        Task VerifyResetTokenAsync(string email, string token);
        public Task ResetPasswordAsync(ResetPasswordDto resetPasswordRequest);
    }
}
