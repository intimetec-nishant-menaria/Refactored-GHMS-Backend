using guest_house_management_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace guest_house_management_backend.Data.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder
                .HasIndex(g => g.Email)
                .IsUnique();

            builder.HasData(
                new Guest
                {
                    Id = 1,
                    Name = "Rahul Sharma",
                    Contact = "9876543210",
                    Email = "rahul@example.com",
                    IDProof = "Aadhar1234",
                    Address = "Delhi",
                    EmergencyContact = "9999999999",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Guest
                {
                    Id = 2,
                    Name = "Priya Verma",
                    Contact = "9123456780",
                    Email = "priya@example.com",
                    IDProof = "Passport5678",
                    Address = "Mumbai",
                    EmergencyContact = "8888888888",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}
