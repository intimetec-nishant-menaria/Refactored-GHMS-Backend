using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

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
                    RoleName = RoleEnum.Admin
                },
                new Role
                {
                    Id = 2,
                    RoleName = RoleEnum.Staff
                },
                new Role
                {
                    Id = 3,
                    RoleName = RoleEnum.Guest
                }
            );
        }
    }
}
