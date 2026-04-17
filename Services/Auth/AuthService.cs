using AutoMapper;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.RoleRepo;
using guest_house_management_backend.Repositories.UserRepo;
using guest_house_management_backend.Repositories.UserTokenRepo;
using guest_house_management_backend.Services.Email;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository,IMapper mapper, IRoleRepository roleRepository, IConfiguration configuration, IEmailSender emailSender, IUserTokenRepository userTokenRepository, IDistributedCache distributedCache)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _emailSender = emailSender;
            _userTokenRepository = userTokenRepository;
            _distributedCache = distributedCache;
            _mapper = mapper;
        }

        public async Task RegisterUserAsync(RegisterDto registerRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(registerRequest.Email);
            if (user != null)
                throw new InvalidOperationException("User with this email already exists.");

            var roleId = await _roleRepository.GetRoleIdByNameAsync(registerRequest.Role);
            User newUser = new User
            {
                Name = registerRequest.FullName,
                Email = registerRequest.Email.ToLower(),
                HashPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                RoleId = roleId,
            };
            await _userRepository.AddUserAsync(newUser);
        }

        public async Task<(string? accessToken , UserResponseDto? user )> AccessTokenAsync(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(refreshToken);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return (null , null);
            }
            var savedToken = await _distributedCache.GetStringAsync($"refresh_{userId}");

            if (savedToken == null || savedToken != refreshToken)
            {
                return (null , null);
            }

            var user = await _userRepository.GetByIdAsync(int.Parse(userId));
            if (user == null || !user.IsActive)
            {
                return (null , null);
            }

            var accessToken = CreateAccessToken(_mapper.Map<UserResponseDto>(user));
            return (accessToken , _mapper.Map<UserResponseDto>(user));

        }

        public async Task<(string accessToken, string refreshToken , User user)> LoginUserAsync(LoginDto loginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email.ToLower());

            if (user == null)
            {
                throw new KeyNotFoundException("User with this email does not exist.");
            }
            if (!user.IsActive || !VerifyUserPassword(loginRequest.Password, user.HashPassword))
            {
                throw new UnauthorizedAccessException("Invalid password Or Inactive Account.");
            }
            var userResponce = _mapper.Map<UserResponseDto>(user);

            var accessToken = CreateAccessToken(userResponce);
            var refreshToken = CreateRefreshToken(userResponce);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
            };

            await _distributedCache.SetStringAsync($"refresh_{user.Id}", refreshToken, cacheOptions);
           
            return (accessToken , refreshToken , user);
        }

        private bool VerifyUserPassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }

        private string CreateRefreshToken(UserResponseDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Role ,user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:RefreshSecret"]!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(15),
                    signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string CreateAccessToken(UserResponseDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role ,user.Role.ToString()),
                new Claim("isActive" ,user.IsActive.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AccessSecret"]!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool Success, string Message)> ChangeUserPassword(int UserId, ChangePasswordDto changePasswordRequest)
        {
            var user = await _userRepository.GetByIdAsync(UserId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            bool valid = BCrypt.Net.BCrypt.Verify(changePasswordRequest.OldPassword, user.HashPassword);

            if (!valid)
                throw new UnauthorizedAccessException("Current password is incorrect.");

            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword);

            await _userRepository.UpdateAsync(user);

            return (true, "Password changed successfully");
        }

        public async Task ForgetUserPasswordAsync(ForgetPasswordDto forgetPasswordRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(forgetPasswordRequest.Email);

            if (user == null)
                throw new KeyNotFoundException("No user found with this email.");

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            var userToken = new UserToken
            {
                UserId = user.Id,
                Token = token,
                Type = UserTokenEnum.ResetPassword,
                Expiry = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userTokenRepository.AddTokenAsync(userToken);
            await _userTokenRepository.SaveChangesAsync();

            var resetLink =
                $"https://localhost:5173/reset-password?email={user.Email}&token={token}";

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

        public async Task VerifyResetTokenAsync(string email, string token)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("No user found with this email.");

            var tokenEntity = await _userTokenRepository.GetValidTokenAsync(
                user.Id,
                token,
                UserTokenEnum.ResetPassword
            );


            if (tokenEntity == null)
                throw new UnauthorizedAccessException("Invalid or expired token.");
        }
        public async Task ResetPasswordAsync(string email , string token ,ResetPasswordDto resetPasswordRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
                throw new KeyNotFoundException("No user found with this email.");

            var tokenEntity = await _userTokenRepository.GetValidTokenAsync(
                user.Id,
                token,
                UserTokenEnum.ResetPassword
            );

            if (tokenEntity == null)
                throw new UnauthorizedAccessException("Invalid or expired reset token.");

            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordRequest.NewPassword);

            tokenEntity.IsUsed = true;

            await _userRepository.SaveChangesAsync();
        }

    }
}


