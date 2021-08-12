using Microsoft.EntityFrameworkCore.Migrations;

namespace BobsBookstore.DataAccess.Migrations
{
    public partial class AddConditionToResale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BookPrice",
                table: "Resale",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ConditionName",
                table: "Resale",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPrice",
                table: "Resale");

            migrationBuilder.DropColumn(
                name: "ConditionName",
                table: "Resale");
        }
    }
}
