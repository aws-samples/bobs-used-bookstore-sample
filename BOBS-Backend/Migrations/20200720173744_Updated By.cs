using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class UpdatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantiy",
                table: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Price",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Price",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Price",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Quantiy",
                table: "Price",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
