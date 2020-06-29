using Microsoft.EntityFrameworkCore.Migrations;

namespace BOBS_Backend.Migrations
{
    public partial class CreateDatabase7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genere_Genere_Id",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Genere");

            migrationBuilder.DropIndex(
                name: "IX_Book_Genere_Id",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Genere_Id",
                table: "Book");

            migrationBuilder.AddColumn<long>(
                name: "Genre_Id",
                table: "Book",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Genre_Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Genre_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_Genre_Id",
                table: "Book",
                column: "Genre_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genre_Genre_Id",
                table: "Book",
                column: "Genre_Id",
                principalTable: "Genre",
                principalColumn: "Genre_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genre_Genre_Id",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_Book_Genre_Id",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Genre_Id",
                table: "Book");

            migrationBuilder.AddColumn<long>(
                name: "Genere_Id",
                table: "Book",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Genere",
                columns: table => new
                {
                    Genere_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genere", x => x.Genere_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_Genere_Id",
                table: "Book",
                column: "Genere_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genere_Genere_Id",
                table: "Book",
                column: "Genere_Id",
                principalTable: "Genere",
                principalColumn: "Genere_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
