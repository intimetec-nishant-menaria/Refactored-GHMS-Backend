using guest_house_management_backend.Enums;
using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace guest_house_management_backend.Data.Configurations
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasData(
                new RoomType
                {
                    Id = 1,
                    RoomTypeName = RoomTypeEnum.Single,
                    Capacity = 1,
                    PricePerNight = 1500,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new RoomType
                {
                    Id = 2,
                    RoomTypeName = RoomTypeEnum.Double,
                    Capacity = 2,
                    PricePerNight = 2500,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new RoomType
                {
                    Id = 3,
                    RoomTypeName = RoomTypeEnum.Suite,
                    Capacity = 4,
                    PricePerNight = 5000,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}
