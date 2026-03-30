using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace guest_house_management_backend.Data.Configurations
{
    public class RoomConfigurations : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasData(
                new Room { Id = 1, RoomNumber = "101", Floor = 1 },
                new Room { Id = 2, RoomNumber = "102", Floor = 1 },

                new Room { Id = 3, RoomNumber = "201", Floor = 2 },
                new Room { Id = 4, RoomNumber = "202", Floor = 2 },

                new Room { Id = 5, RoomNumber = "301", Floor = 3 },
                new Room { Id = 6, RoomNumber = "401", Floor = 4 }
            );
        }
    }
}
