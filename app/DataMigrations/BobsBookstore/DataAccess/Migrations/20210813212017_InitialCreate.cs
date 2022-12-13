//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//namespace DataAccess.Migrations
//{
//    public partial class InitialCreate : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "Condition",
//                columns: table => new
//                {
//                    Condition_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    ConditionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Condition", x => x.Condition_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Customer",
//                columns: table => new
//                {
//                    Customer_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Customer", x => x.Customer_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Genre",
//                columns: table => new
//                {
//                    Genre_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Genre", x => x.Genre_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "OrderStatus",
//                columns: table => new
//                {
//                    OrderStatus_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Position = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_OrderStatus", x => x.OrderStatus_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Publisher",
//                columns: table => new
//                {
//                    Publisher_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Publisher", x => x.Publisher_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "ResaleStatus",
//                columns: table => new
//                {
//                    ResaleStatus_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_ResaleStatus", x => x.ResaleStatus_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Type",
//                columns: table => new
//                {
//                    Type_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Type", x => x.Type_Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "Address",
//                columns: table => new
//                {
//                    Address_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
//                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Customer_Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Address", x => x.Address_Id);
//                    table.ForeignKey(
//                        name: "FK_Address_Customer_Customer_Id",
//                        column: x => x.Customer_Id,
//                        principalTable: "Customer",
//                        principalColumn: "Customer_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Cart",
//                columns: table => new
//                {
//                    Cart_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Customer_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
//                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Cart", x => x.Cart_Id);
//                    table.ForeignKey(
//                        name: "FK_Cart_Customer_Customer_Id",
//                        column: x => x.Customer_Id,
//                        principalTable: "Customer",
//                        principalColumn: "Customer_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Resale",
//                columns: table => new
//                {
//                    Resale_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    FrontUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    BackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    LeftUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RightUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    AudioBookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PublisherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ResaleStatus_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Comment = table.Column<string>(type: "varchar(MAX)", nullable: true),
//                    Customer_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
//                    BookPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
//                    ConditionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Resale", x => x.Resale_Id);
//                    table.ForeignKey(
//                        name: "FK_Resale_Customer_Customer_Id",
//                        column: x => x.Customer_Id,
//                        principalTable: "Customer",
//                        principalColumn: "Customer_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Resale_ResaleStatus_ResaleStatus_Id",
//                        column: x => x.ResaleStatus_Id,
//                        principalTable: "ResaleStatus",
//                        principalColumn: "ResaleStatus_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Book",
//                columns: table => new
//                {
//                    Book_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Publisher_Id = table.Column<long>(type: "bigint", nullable: true),
//                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Type_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Genre_Id = table.Column<long>(type: "bigint", nullable: true),
//                    FrontUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    BackUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    LeftUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RightUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    AudioBookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Book", x => x.Book_Id);
//                    table.ForeignKey(
//                        name: "FK_Book_Genre_Genre_Id",
//                        column: x => x.Genre_Id,
//                        principalTable: "Genre",
//                        principalColumn: "Genre_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Book_Publisher_Publisher_Id",
//                        column: x => x.Publisher_Id,
//                        principalTable: "Publisher",
//                        principalColumn: "Publisher_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Book_Type_Type_Id",
//                        column: x => x.Type_Id,
//                        principalTable: "Type",
//                        principalColumn: "Type_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Order",
//                columns: table => new
//                {
//                    Order_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
//                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
//                    DeliveryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    OrderStatus_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Customer_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
//                    Address_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Rowversion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Order", x => x.Order_Id);
//                    table.ForeignKey(
//                        name: "FK_Order_Address_Address_Id",
//                        column: x => x.Address_Id,
//                        principalTable: "Address",
//                        principalColumn: "Address_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Order_Customer_Customer_Id",
//                        column: x => x.Customer_Id,
//                        principalTable: "Customer",
//                        principalColumn: "Customer_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Order_OrderStatus_OrderStatus_Id",
//                        column: x => x.OrderStatus_Id,
//                        principalTable: "OrderStatus",
//                        principalColumn: "OrderStatus_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "Price",
//                columns: table => new
//                {
//                    Price_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Book_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Condition_Id = table.Column<long>(type: "bigint", nullable: true),
//                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    Active = table.Column<bool>(type: "bit", nullable: false),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_Price", x => x.Price_Id);
//                    table.ForeignKey(
//                        name: "FK_Price_Book_Book_Id",
//                        column: x => x.Book_Id,
//                        principalTable: "Book",
//                        principalColumn: "Book_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_Price_Condition_Condition_Id",
//                        column: x => x.Condition_Id,
//                        principalTable: "Condition",
//                        principalColumn: "Condition_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "CartItem",
//                columns: table => new
//                {
//                    CartItem_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Price_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Cart_Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
//                    Book_Id = table.Column<long>(type: "bigint", nullable: true),
//                    WantToBuy = table.Column<bool>(type: "bit", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_CartItem", x => x.CartItem_Id);
//                    table.ForeignKey(
//                        name: "FK_CartItem_Book_Book_Id",
//                        column: x => x.Book_Id,
//                        principalTable: "Book",
//                        principalColumn: "Book_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_CartItem_Cart_Cart_Id",
//                        column: x => x.Cart_Id,
//                        principalTable: "Cart",
//                        principalColumn: "Cart_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_CartItem_Price_Price_Id",
//                        column: x => x.Price_Id,
//                        principalTable: "Price",
//                        principalColumn: "Price_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "OrderDetail",
//                columns: table => new
//                {
//                    OrderDetail_Id = table.Column<long>(type: "bigint", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Order_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Book_Id = table.Column<long>(type: "bigint", nullable: true),
//                    Price_Id = table.Column<long>(type: "bigint", nullable: true),
//                    OrderDetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
//                    Quantity = table.Column<int>(type: "int", nullable: false),
//                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
//                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetail_Id);
//                    table.ForeignKey(
//                        name: "FK_OrderDetail_Book_Book_Id",
//                        column: x => x.Book_Id,
//                        principalTable: "Book",
//                        principalColumn: "Book_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_OrderDetail_Order_Order_Id",
//                        column: x => x.Order_Id,
//                        principalTable: "Order",
//                        principalColumn: "Order_Id",
//                        onDelete: ReferentialAction.Restrict);
//                    table.ForeignKey(
//                        name: "FK_OrderDetail_Price_Price_Id",
//                        column: x => x.Price_Id,
//                        principalTable: "Price",
//                        principalColumn: "Price_Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_Address_Customer_Id",
//                table: "Address",
//                column: "Customer_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Book_Genre_Id",
//                table: "Book",
//                column: "Genre_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Book_Publisher_Id",
//                table: "Book",
//                column: "Publisher_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Book_Type_Id",
//                table: "Book",
//                column: "Type_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Cart_Customer_Id",
//                table: "Cart",
//                column: "Customer_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_CartItem_Book_Id",
//                table: "CartItem",
//                column: "Book_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_CartItem_Cart_Id",
//                table: "CartItem",
//                column: "Cart_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_CartItem_Price_Id",
//                table: "CartItem",
//                column: "Price_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Order_Address_Id",
//                table: "Order",
//                column: "Address_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Order_Customer_Id",
//                table: "Order",
//                column: "Customer_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Order_OrderStatus_Id",
//                table: "Order",
//                column: "OrderStatus_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderDetail_Book_Id",
//                table: "OrderDetail",
//                column: "Book_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderDetail_Order_Id",
//                table: "OrderDetail",
//                column: "Order_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_OrderDetail_Price_Id",
//                table: "OrderDetail",
//                column: "Price_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Price_Book_Id",
//                table: "Price",
//                column: "Book_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Price_Condition_Id",
//                table: "Price",
//                column: "Condition_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Resale_Customer_Id",
//                table: "Resale",
//                column: "Customer_Id");

//            migrationBuilder.CreateIndex(
//                name: "IX_Resale_ResaleStatus_Id",
//                table: "Resale",
//                column: "ResaleStatus_Id");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "CartItem");

//            migrationBuilder.DropTable(
//                name: "OrderDetail");

//            migrationBuilder.DropTable(
//                name: "Resale");

//            migrationBuilder.DropTable(
//                name: "Cart");

//            migrationBuilder.DropTable(
//                name: "Order");

//            migrationBuilder.DropTable(
//                name: "Price");

//            migrationBuilder.DropTable(
//                name: "ResaleStatus");

//            migrationBuilder.DropTable(
//                name: "Address");

//            migrationBuilder.DropTable(
//                name: "OrderStatus");

//            migrationBuilder.DropTable(
//                name: "Book");

//            migrationBuilder.DropTable(
//                name: "Condition");

//            migrationBuilder.DropTable(
//                name: "Customer");

//            migrationBuilder.DropTable(
//                name: "Genre");

//            migrationBuilder.DropTable(
//                name: "Publisher");

//            migrationBuilder.DropTable(
//                name: "Type");
//        }
//    }
//}
