using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookstoreBackend.Migrations
{
    public partial class concurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Type",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Publisher",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Genre",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Condition",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Book",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Type");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Publisher");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Book");
        }
    }
}
