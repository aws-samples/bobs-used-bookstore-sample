using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class CreateDatabase11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudoBook_Url",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "AudioBook_Url",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioBook_Url",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "AudoBook_Url",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
