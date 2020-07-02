using Microsoft.EntityFrameworkCore.Migrations;

namespace BobBookstore.Migrations
{
    public partial class GenreForeignKeyy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genre_Genre_Id",
                table: "Book");

            migrationBuilder.AlterColumn<long>(
                name: "Genre_Id",
                table: "Book",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genre_Genre_Id",
                table: "Book",
                column: "Genre_Id",
                principalTable: "Genre",
                principalColumn: "Genre_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genre_Genre_Id",
                table: "Book");

            migrationBuilder.AlterColumn<long>(
                name: "Genre_Id",
                table: "Book",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genre_Genre_Id",
                table: "Book",
                column: "Genre_Id",
                principalTable: "Genre",
                principalColumn: "Genre_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
