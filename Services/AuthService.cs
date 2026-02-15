using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace guest_house_management_backend.Services
{   
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository ,IRoleRepository roleRepository ,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
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
                    issuer: _configuration["JWt:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(5),
                    signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
