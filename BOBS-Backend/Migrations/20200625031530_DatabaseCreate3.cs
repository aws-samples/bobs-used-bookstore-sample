using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class DatabaseCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Address_Address_Id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_Customer_Id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_Address_Id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_Customer_Id",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "Customer_Id",
                table: "Order",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Address_Id",
                table: "Order",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Address_Id",
                table: "Order",
                column: "Address_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Customer_Id",
                table: "Order",
                column: "Customer_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Address_Address_Id",
                table: "Order",
                column: "Address_Id",
                principalTable: "Address",
                principalColumn: "Address_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_Customer_Id",
                table: "Order",
                column: "Customer_Id",
                principalTable: "Customer",
                principalColumn: "Customer_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Address_Address_Id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_Customer_Id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_Address_Id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_Customer_Id",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "Customer_Id",
                table: "Order",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Address_Id",
                table: "Order",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Address_Id",
                table: "Order",
                column: "Address_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Customer_Id",
                table: "Order",
                column: "Customer_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Address_Address_Id",
                table: "Order",
                column: "Address_Id",
                principalTable: "Address",
                principalColumn: "Address_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_Customer_Id",
                table: "Order",
                column: "Customer_Id",
                principalTable: "Customer",
                principalColumn: "Customer_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
