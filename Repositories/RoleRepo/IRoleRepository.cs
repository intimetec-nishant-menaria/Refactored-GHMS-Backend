using guest_house_management_backend.Enums;

namespace guest_house_management_backend.Repositories.RoleRepo
{
    public interface IRoleRepository
    {
        public Task<int> GetRoleIdByNameAsync(string roleName);
    }
}
