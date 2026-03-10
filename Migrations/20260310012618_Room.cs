using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace guest_house_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class Room : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FloorNumber",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "Room");
        }
    }
}
