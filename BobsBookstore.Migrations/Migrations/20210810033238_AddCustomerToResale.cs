using Microsoft.EntityFrameworkCore.Migrations;

namespace BobsBookstore.DataAccess.Migrations
{
    public partial class AddCustomerToResale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Customer_Id",
                table: "Resale",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resale_Customer_Id",
                table: "Resale",
                column: "Customer_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resale_Customer_Customer_Id",
                table: "Resale",
                column: "Customer_Id",
                principalTable: "Customer",
                principalColumn: "Customer_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resale_Customer_Customer_Id",
                table: "Resale");

            migrationBuilder.DropIndex(
                name: "IX_Resale_Customer_Id",
                table: "Resale");

            migrationBuilder.DropColumn(
                name: "Customer_Id",
                table: "Resale");
        }
    }
}
