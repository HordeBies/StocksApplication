using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stocks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNavigationPropertiesForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SellOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BuyOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SellOrders_UserId",
                table: "SellOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyOrders_UserId",
                table: "BuyOrders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyOrders_AspNetUsers_UserId",
                table: "BuyOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellOrders_AspNetUsers_UserId",
                table: "SellOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyOrders_AspNetUsers_UserId",
                table: "BuyOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SellOrders_AspNetUsers_UserId",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_SellOrders_UserId",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_BuyOrders_UserId",
                table: "BuyOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SellOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BuyOrders");
        }
    }
}
