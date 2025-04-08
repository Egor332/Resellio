using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellioBackend.Migrations
{
    /// <inheritdoc />
    public partial class SellerColumnRenamedInTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Tickets",
                newName: "HolderId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_SellerId",
                table: "Tickets",
                newName: "IX_Tickets_HolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_HolderId",
                table: "Tickets",
                column: "HolderId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_HolderId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "HolderId",
                table: "Tickets",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_HolderId",
                table: "Tickets",
                newName: "IX_Tickets_SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
