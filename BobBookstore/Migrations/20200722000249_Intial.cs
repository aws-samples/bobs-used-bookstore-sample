using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BobBookstore.Migrations
{
    public partial class Intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetail_Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Id = table.Column<long>(nullable: true),
                    Book_Id = table.Column<long>(nullable: true),
                    Price_Id = table.Column<long>(nullable: true),
                    price = table.Column<double>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetail_Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Book_Book_Id",
                        column: x => x.Book_Id,
                        principalTable: "Book",
                        principalColumn: "Book_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "Order",
                        principalColumn: "Order_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Price_Price_Id",
                        column: x => x.Price_Id,
                        principalTable: "Price",
                        principalColumn: "Price_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Book_Id",
                table: "OrderDetail",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Order_Id",
                table: "OrderDetail",
                column: "Order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_Price_Id",
                table: "OrderDetail",
                column: "Price_Id");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(
                name: "OrderDetail");

            
        }
    }
}
