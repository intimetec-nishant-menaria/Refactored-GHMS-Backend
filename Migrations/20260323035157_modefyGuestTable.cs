using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace guest_house_management_backend.Migrations
{
    /// <inheritdoc />
    public partial class modefyGuestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Guest_Contact",
                table: "Guest");

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "Guest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "Guest",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_Contact",
                table: "Guest",
                column: "Contact",
                unique: true);
        }
    }
}
