using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResellioBackend.Migrations
{
    /// <inheritdoc />
    public partial class MakeSellerNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_SellerId",
                table: "Tickets",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
