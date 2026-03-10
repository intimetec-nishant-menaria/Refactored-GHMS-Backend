using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace guest_house_management_backend.Data.Configurations
{
    public class RoomAmenityConfiguration : IEntityTypeConfiguration<RoomAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomAmenity> builder)
        {
            builder.HasData(
                new RoomAmenity { Id = 1, Name = "WiFi" },
                new RoomAmenity { Id = 2, Name = "Air Conditioning" },
                new RoomAmenity { Id = 3, Name = "Television" },
                new RoomAmenity { Id = 4, Name = "Mini Bar" }
            );
        }
    }
}
