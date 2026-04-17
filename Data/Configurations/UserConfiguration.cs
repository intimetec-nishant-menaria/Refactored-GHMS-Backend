using guest_house_management_backend.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace guest_house_management_backend.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@intimetec.com",
                    HashPassword = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    IsActive = true,
                    RoleId = 4
                }
            );
        }
    }
}
