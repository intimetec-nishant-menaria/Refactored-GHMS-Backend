using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;
using guest_house_management_backend.Repositories;

namespace guest_house_management_backend.Services
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

        public async Task CreateUserAsync(CreateUserDto user)
        {
            int RoleID = await _roleRepository.GetRoleIdByNameAsync(user.Role);

            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password),
                RoleId = RoleID,
            };

            await _userRepository.AddUserAsync(newUser);
        }

        public async Task DeleteUserAsync(int Id)
        {
            await _userRepository.DeleteAsync(Id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}
