using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class CreateDatabase9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudoBook_Url",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Front_Url",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Right_Url",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudoBook_Url",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Front_Url",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Right_Url",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Book");
        }
    }
}
