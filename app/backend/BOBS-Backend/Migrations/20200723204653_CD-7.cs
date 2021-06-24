using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookstoreBackend.Migrations
{
    public partial class CD7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Refund",
                table: "Order");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Price",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrderDetail",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Order",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Order");

            migrationBuilder.AddColumn<double>(
                name: "Refund",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
