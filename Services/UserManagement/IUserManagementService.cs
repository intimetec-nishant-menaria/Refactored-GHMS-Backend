using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.UserManagement
{
    public interface IUserManagementService
    {
        public Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        public Task<User?> GetUserByIdAsync(int id);

        public Task CreateUserAsync(CreateUserDto userDto);

        public Task DeleteUserAsync(int Id);


    }
}
