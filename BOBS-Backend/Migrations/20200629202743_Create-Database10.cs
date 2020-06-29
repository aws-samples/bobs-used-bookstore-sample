using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class CreateDatabase10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Back_Url",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Left_Url",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Back_Url",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Left_Url",
                table: "Book");
        }
    }
}
