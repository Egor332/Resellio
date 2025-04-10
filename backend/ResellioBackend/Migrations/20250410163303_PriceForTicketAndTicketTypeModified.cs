using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellioBackend.Migrations
{
    /// <inheritdoc />
    public partial class PriceForTicketAndTicketTypeModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "TicketTypes");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "TicketTypes",
                newName: "PriceAmount");

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "TicketTypes",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAmount",
                table: "Tickets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "Tickets",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "TicketTypes");

            migrationBuilder.DropColumn(
                name: "PriceAmount",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "PriceAmount",
                table: "TicketTypes",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "TicketTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
