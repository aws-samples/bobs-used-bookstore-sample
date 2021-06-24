using Microsoft.EntityFrameworkCore.Migrations;

namespace BookstoreBackend.Migrations
{
    public partial class CD8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "position",
                table: "OrderStatus",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "OrderStatus");
        }
    }
}
