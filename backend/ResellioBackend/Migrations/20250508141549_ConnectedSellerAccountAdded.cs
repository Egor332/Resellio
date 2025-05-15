using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellioBackend.Migrations
{
    /// <inheritdoc />
    public partial class ConnectedSellerAccountAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConnectedSellingAccount",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectedSellingAccount",
                table: "Users");
        }
    }
}
