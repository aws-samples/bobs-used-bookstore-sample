using Microsoft.EntityFrameworkCore.Migrations;

namespace BobBookstore.Migrations
{
    public partial class Initializetakethree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Genres_Genre_Id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_Publishers_Publisher_Id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_Types_Type_Id",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Book_Book_Id",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Book_Book_Id",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Book_Book_Id",
                table: "Prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Book_Type_Id",
                table: "Books",
                newName: "IX_Books_Type_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Book_Publisher_Id",
                table: "Books",
                newName: "IX_Books_Publisher_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Book_Genre_Id",
                table: "Books",
                newName: "IX_Books_Genre_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_Genre_Id",
                table: "Books",
                column: "Genre_Id",
                principalTable: "Genres",
                principalColumn: "Genre_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_Publisher_Id",
                table: "Books",
                column: "Publisher_Id",
                principalTable: "Publishers",
                principalColumn: "Publisher_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Types_Type_Id",
                table: "Books",
                column: "Type_Id",
                principalTable: "Types",
                principalColumn: "Type_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Books_Book_Id",
                table: "CartItems",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Books_Book_Id",
                table: "OrderDetails",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Books_Book_Id",
                table: "Prices",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_Genre_Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_Publisher_Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Types_Type_Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Books_Book_Id",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Books_Book_Id",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Books_Book_Id",
                table: "Prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Type_Id",
                table: "Book",
                newName: "IX_Book_Type_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Publisher_Id",
                table: "Book",
                newName: "IX_Book_Publisher_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Genre_Id",
                table: "Book",
                newName: "IX_Book_Genre_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "Book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Genres_Genre_Id",
                table: "Book",
                column: "Genre_Id",
                principalTable: "Genres",
                principalColumn: "Genre_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Publishers_Publisher_Id",
                table: "Book",
                column: "Publisher_Id",
                principalTable: "Publishers",
                principalColumn: "Publisher_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Types_Type_Id",
                table: "Book",
                column: "Type_Id",
                principalTable: "Types",
                principalColumn: "Type_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Book_Book_Id",
                table: "CartItems",
                column: "Book_Id",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Book_Book_Id",
                table: "OrderDetails",
                column: "Book_Id",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Book_Book_Id",
                table: "Prices",
                column: "Book_Id",
                principalTable: "Book",
                principalColumn: "Book_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
