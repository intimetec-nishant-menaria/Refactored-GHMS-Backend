using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace guest_house_management_backend.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = 1,
                    RoleName = "Ops"
                },
                new Role
                {
                    Id = 2,
                    RoleName = "HR"
                },
                new Role
                {
                    Id = 3,
                    RoleName = "Guard"
                },
                new Role
                {
                    Id = 4,
                    RoleName = "Admin"
                }
            );
        }
    }
}
