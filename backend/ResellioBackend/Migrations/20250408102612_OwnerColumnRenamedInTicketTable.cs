using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellioBackend.Migrations
{
    /// <inheritdoc />
    public partial class OwnerColumnRenamedInTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_OwnerId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Tickets",
                newName: "PurchaseIntenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_OwnerId",
                table: "Tickets",
                newName: "IX_Tickets_PurchaseIntenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_PurchaseIntenderId",
                table: "Tickets",
                column: "PurchaseIntenderId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_PurchaseIntenderId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "PurchaseIntenderId",
                table: "Tickets",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_PurchaseIntenderId",
                table: "Tickets",
                newName: "IX_Tickets_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_OwnerId",
                table: "Tickets",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
