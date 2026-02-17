using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.RoleRepo;
using guest_house_management_backend.Repositories.UserRepo;
using guest_house_management_backend.Repositories.UserTokenRepo;
using guest_house_management_backend.Services.Email;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace guest_house_management_backend.Services.Auth
{   
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IUserTokenRepository _userTokenRepository;

        public AuthService(IUserRepository userRepository ,IRoleRepository roleRepository ,IConfiguration configuration , IEmailSender emailSender , IUserTokenRepository userTokenRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _emailSender = emailSender;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user!=null)
                return false;

            var RoleId = await _roleRepository.GetRoleIdByNameAsync("guest");

             User newUser = new User
            {
                Name = dto.FullName,
                Email = dto.Email,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = RoleId,
            };

            await _userRepository.AddUserAsync(newUser);
            return true;
        }


        public async Task<string?> LoginAsync(LoginDto Dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(Dto.Email);

            if (user == null)
                return null;

            if (!VerifyPassword(Dto.Password, user.HashPassword))
                return null;


            return CreateToken(user);
        }


        private bool VerifyPassword(string password , string hashPassword) 
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }


        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role ,user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(5),
                    signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool Success,string Message)> ChangePassword(int UserId,ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(UserId);

            if (user == null)
                return (false, "User not found");

            bool valid = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.HashPassword);

            if (!valid)
                return (false , "current password is incorrect");

            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _userRepository.UpdateAsync(user);

            return (true, "Password changed successfully");
        }

        public async Task ForgetPasswordAsync(ForgetPasswordDto forgetPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(forgetPassword.Email);

            if (user == null)
                return;

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var userToken = new UserToken
            {
                UserId = user.Id,
                Token = token,
                Type = TokenType.ResetPassword,
                Expiry = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userTokenRepository.AddTokenAsync(userToken);
            await _userTokenRepository.SaveChangesAsync();

            var resetLink =
                $"http://localhost:5173/verify-reset-password?email={user.Email}&token={token}";

            await _emailSender.SendEmailASync(
                user.Email,
                "Reset Your Password",
                $"""
                <h3>Password Reset</h3>
                <p>Click below to reset your password:</p>
                <a href='{resetLink}'>Reset Password</a>
                <p>This link expires in 30 minutes.</p>
                """
             );
        }

        public async Task<bool> VerifyResetTokenAsync(string email, string token)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return false;

            var tokenEntity = await _userTokenRepository.GetValidTokenAsync(
                user.Id,
                token,
                TokenType.ResetPassword
            );

            return tokenEntity != null;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return false;

            var tokenEntity = await _userTokenRepository.GetValidTokenAsync(
                user.Id,
                dto.Token,
                TokenType.ResetPassword
            );

            if (tokenEntity == null)
                return false;

            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            tokenEntity.IsUsed = true;

            await _userRepository.SaveChangesAsync();

            return true;
        }

    }
}


