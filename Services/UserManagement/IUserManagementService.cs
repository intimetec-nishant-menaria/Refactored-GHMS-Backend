using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Services.UserManagement
{
    public interface IUserManagementService
    {
        public Task<Paging<UserResponseDto>> GetAllUsersAsync(int pageNumber , int pageSize , string searchUser);

        public Task<User?> GetUserByIdAsync(int id);

        public Task CreateUserAsync(CreateUserDto userDto);

        public Task DeleteUserAsync(int Id);

        public Task UpdateUserAsync(int id, UpdateUserDto dto);

    }
}
