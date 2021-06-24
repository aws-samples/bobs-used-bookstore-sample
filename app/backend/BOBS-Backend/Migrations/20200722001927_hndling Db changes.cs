using Microsoft.EntityFrameworkCore.Migrations;

namespace BookstoreBackend.Migrations
{
    public partial class hndlingDbchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "OrderDetail",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "price",
                table: "OrderDetail",
                type: "float",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
