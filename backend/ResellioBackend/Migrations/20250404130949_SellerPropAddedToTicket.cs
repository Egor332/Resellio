using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellioBackend.Migrations
{
    /// <inheritdoc />
    public partial class SellerPropAddedToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SellerId",
                table: "Tickets",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SellerId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Tickets");
        }
    }
}
