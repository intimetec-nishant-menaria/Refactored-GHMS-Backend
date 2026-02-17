using guest_house_management_backend.Data;
using guest_house_management_backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.RoleRepo
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Data.DBContext _context;

        public RoleRepository(Data.DBContext context) {
            _context = context;
        }

        public async Task<Guid> GetRoleIdByNameAsync(RoleEnum roleName)
        {
            var role = await _context.Roles
                 .FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role == null)
                throw new KeyNotFoundException("Role not found.");

            return role.Id;
        }
    }
}
