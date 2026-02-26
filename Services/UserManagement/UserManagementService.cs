using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories.RoleRepo;
using guest_house_management_backend.Repositories.UserRepo;

namespace guest_house_management_backend.Services.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserManagementService(IUserRepository userRepository , IRoleRepository roleRepository) 
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task CreateUserAsync(CreateUserDto userDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }
            int roleId = await _roleRepository.GetRoleIdByNameAsync(userDto.Role);
       
            User newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                RoleId = roleId,
            };

            await _userRepository.AddUserAsync(newUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return user;
        }
    }
}
