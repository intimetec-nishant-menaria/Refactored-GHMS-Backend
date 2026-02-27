using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace guest_house_management_backend.Data.Configurations
{
    public class RoomTypeAmenityConfiguration : IEntityTypeConfiguration<RoomTypeAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomTypeAmenity> builder)
        {
            builder.HasKey(rta => new { rta.RoomTypeId, rta.AmenityId });

            builder.HasOne(rta => rta.RoomType)
                .WithMany(rt => rt.RoomTypeAmenities)
                .HasForeignKey(rta => rta.RoomTypeId);

            builder.HasOne(rta => rta.Amenity)
                .WithMany(a => a.RoomTypeAmenities)
                .HasForeignKey(rta => rta.AmenityId);

            builder.HasData(
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