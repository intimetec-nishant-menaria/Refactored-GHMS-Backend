using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
namespace guest_house_management_backend.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomAmenity> Amenities { get; set; }
        public DbSet<RoomTypeAmenity> RoomTypeAmenities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    RoleName = RoleEnum.Admin
                },
                new Role
                {
                    Id =2,
                    RoleName = RoleEnum.Staff
                },
                new Role
                {
                    Id =3,
                    RoleName = RoleEnum.Guest
                }
            );



            modelBuilder.Entity<RoomAmenity>().HasData(
                new RoomAmenity
                {
                    Id = 1,
                    Name = "WiFi"
                },
                new RoomAmenity
                {
                    Id = 2,
                    Name = "Air Conditioning"
                },
                new RoomAmenity
                {
                    Id = 3,
                    Name = "Television"
                },
                new RoomAmenity
                {
                    Id = 4,
                    Name = "Mini Bar"
                }
            );

            modelBuilder.Entity<RoomType>().HasData(
                new RoomType
                {
                    Id = 1,
                    RoomTypeName = RoomTypeEnum.Single,
                    Capacity = 1,
                    PricePerNight = 1500,
                    CreatedAt = DateTime.UtcNow
                },
                new RoomType
                {
                    Id = 2,
                    RoomTypeName = RoomTypeEnum.Double,
                    Capacity = 2,
                    PricePerNight = 2500,
                    CreatedAt = DateTime.UtcNow
                },
                new RoomType
                {
                    Id = 3,
                    RoomTypeName = RoomTypeEnum.Suite,
                    Capacity = 4,
                    PricePerNight = 5000,
                    CreatedAt = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<RoomTypeAmenity>()
                .HasKey(rta => new { rta.RoomTypeId, rta.AmenityId });

            modelBuilder.Entity<RoomTypeAmenity>()
                .HasOne(rta => rta.RoomType)
                .WithMany(rt => rt.RoomTypeAmenities)
                .HasForeignKey(rta => rta.RoomTypeId);

            modelBuilder.Entity<RoomTypeAmenity>()
                .HasOne(rta => rta.Amenity)
                .WithMany(a => a.RoomTypeAmenities)
                .HasForeignKey(rta => rta.AmenityId);

            modelBuilder.Entity<RoomTypeAmenity>().HasData(

                new { RoomTypeId = 1, AmenityId = 1 },

                new { RoomTypeId = 2, AmenityId = 1 },
                new { RoomTypeId = 2, AmenityId = 2 },

                new { RoomTypeId = 3, AmenityId = 1 },
                new { RoomTypeId = 3, AmenityId = 2 },
                new { RoomTypeId = 3, AmenityId = 3 },
                new { RoomTypeId = 3, AmenityId = 4 }
            );
        }
    }
}
