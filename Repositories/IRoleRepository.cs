namespace guest_house_management_backend.Repositories
{
    public interface IRoleRepository
    {
        public Task<int> GetRoleIdByNameAsync(string roleName);
    }
}
