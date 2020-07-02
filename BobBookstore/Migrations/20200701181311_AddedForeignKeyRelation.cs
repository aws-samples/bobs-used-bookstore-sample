using Microsoft.EntityFrameworkCore.Migrations;

namespace BobBookstore.Migrations
{
    public partial class AddedForeignKeyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Genre_Id1",
                table: "Book",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_Genre_Id1",
                table: "Book",
                column: "Genre_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genre_Genre_Id1",
                table: "Book",
                column: "Genre_Id1",
                principalTable: "Genre",
                principalColumn: "Genre_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genre_Genre_Id1",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_Genre_Id1",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Genre_Id1",
                table: "Book");
        }
    }
}
