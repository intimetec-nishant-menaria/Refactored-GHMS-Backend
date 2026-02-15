using guest_house_management_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MyDbContext _context;

        public RoleRepository(MyDbContext context) {
            _context = context;
        }

        public async Task<int> GetRoleIdByNameAsync(string roleName)
        {
            var role  = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            return role!.Id;
        }
    }
}
