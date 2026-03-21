using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace guest_house_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class updatingBookingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualCheckOutTime",
                table: "Bookings",
                newName: "CheckOutTime");

            migrationBuilder.RenameColumn(
                name: "ActualCheckInTime",
                table: "Bookings",
                newName: "CheckInTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOutTime",
                table: "Bookings",
                newName: "ActualCheckOutTime");

            migrationBuilder.RenameColumn(
                name: "CheckInTime",
                table: "Bookings",
                newName: "ActualCheckInTime");
        }
    }
}
