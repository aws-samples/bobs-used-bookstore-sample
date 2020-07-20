using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class CD4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "OrderDetail",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "OrderDetail");
        }
    }
}
