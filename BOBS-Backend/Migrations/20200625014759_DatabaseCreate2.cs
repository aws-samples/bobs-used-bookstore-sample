using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class DatabaseCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_orderStatus_OrderStatus_Id",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderStatus",
                table: "orderStatus");

            migrationBuilder.RenameTable(
                name: "orderStatus",
                newName: "OrderStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStatus",
                table: "OrderStatus",
                column: "OrderStatus_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderStatus_OrderStatus_Id",
                table: "Order",
                column: "OrderStatus_Id",
                principalTable: "OrderStatus",
                principalColumn: "OrderStatus_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderStatus_OrderStatus_Id",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStatus",
                table: "OrderStatus");

            migrationBuilder.RenameTable(
                name: "OrderStatus",
                newName: "orderStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderStatus",
                table: "orderStatus",
                column: "OrderStatus_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_orderStatus_OrderStatus_Id",
                table: "Order",
                column: "OrderStatus_Id",
                principalTable: "orderStatus",
                principalColumn: "OrderStatus_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
