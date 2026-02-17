using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }    
    }
}
