using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
namespace guest_house_management_backend.Data
{
    public class DBContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DBContext(DbContextOptions options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }    
        public DbSet<Room> Rooms { get; set; }
    }
}
