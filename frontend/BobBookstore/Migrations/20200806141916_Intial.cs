using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BobBookstore.Migrations
{
    public partial class Intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItem_Id = table.Column<string>(nullable: false),
                    Price_Id = table.Column<long>(nullable: true),
                    Cart_Id = table.Column<string>(nullable: true),
                    Book_Id = table.Column<long>(nullable: true),
                    WantToBuy = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.CartItem_Id);
                    table.ForeignKey(
                        name: "FK_CartItem_Book_Book_Id",
                        column: x => x.Book_Id,
                        principalTable: "Book",
                        principalColumn: "Book_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_Cart_Id",
                        column: x => x.Cart_Id,
                        principalTable: "Cart",
                        principalColumn: "Cart_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItem_Price_Price_Id",
                        column: x => x.Price_Id,
                        principalTable: "Price",
                        principalColumn: "Price_Id",
                        onDelete: ReferentialAction.Restrict);
                });

           
            migrationBuilder.CreateIndex(
                name: "IX_CartItem_Book_Id",
                table: "CartItem",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_Cart_Id",
                table: "CartItem",
                column: "Cart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_Price_Id",
                table: "CartItem",
                column: "Price_Id");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropTable(
                name: "Type");
        }
    }
}
