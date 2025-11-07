using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookstore.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bobsusedbookstore_dbo");

            migrationBuilder.CreateTable(
                name: "author",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    business_entity_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    national_id_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    login_id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    job_title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    marital_status = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    gender = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    hire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    vacation_hours = table.Column<short>(type: "smallint", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author", x => x.business_entity_id);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sub = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    product_number = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    safety_stock_level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.product_id);
                });

            migrationBuilder.CreateTable(
                name: "reference_data",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data_type = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reference_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shopping_cart",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    correlation_id = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_cart", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "address",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address_line1 = table.Column<string>(type: "text", nullable: false),
                    address_line2 = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "book",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: true),
                    isbn = table.Column<string>(type: "text", nullable: false),
                    publisher_id = table.Column<int>(type: "integer", nullable: false),
                    book_type_id = table.Column<int>(type: "integer", nullable: false),
                    genre_id = table.Column<int>(type: "integer", nullable: false),
                    condition_id = table.Column<int>(type: "integer", nullable: false),
                    cover_image_url = table.Column<string>(type: "text", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.id);
                    table.ForeignKey(
                        name: "FK_book_reference_data_book_type_id",
                        column: x => x.book_type_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_reference_data_condition_id",
                        column: x => x.condition_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_reference_data_genre_id",
                        column: x => x.genre_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_reference_data_publisher_id",
                        column: x => x.publisher_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "offer",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    author = table.Column<string>(type: "text", nullable: false),
                    isbn = table.Column<string>(type: "text", nullable: false),
                    book_name = table.Column<string>(type: "text", nullable: false),
                    front_url = table.Column<string>(type: "text", nullable: true),
                    genre_id = table.Column<int>(type: "integer", nullable: false),
                    condition_id = table.Column<int>(type: "integer", nullable: false),
                    publisher_id = table.Column<int>(type: "integer", nullable: false),
                    book_type_id = table.Column<int>(type: "integer", nullable: false),
                    summary = table.Column<string>(type: "text", nullable: true),
                    offer_status = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    book_price = table.Column<decimal>(type: "numeric", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offer", x => x.id);
                    table.ForeignKey(
                        name: "FK_offer_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_offer_reference_data_book_type_id",
                        column: x => x.book_type_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_offer_reference_data_condition_id",
                        column: x => x.condition_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_offer_reference_data_genre_id",
                        column: x => x.genre_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_offer_reference_data_publisher_id",
                        column: x => x.publisher_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "reference_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    order_status = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_address_address_id",
                        column: x => x.address_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shopping_cart_item",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shopping_cart_id = table.Column<int>(type: "integer", nullable: false),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    want_to_buy = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_cart_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_cart_item_book_book_id",
                        column: x => x.book_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shopping_cart_item_shopping_cart_shopping_cart_id",
                        column: x => x.shopping_cart_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "shopping_cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                schema: "bobsusedbookstore_dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_item_book_book_id",
                        column: x => x.book_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_item_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "bobsusedbookstore_dbo",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "bobsusedbookstore_dbo",
                table: "reference_data",
                columns: new[] { "id", "created_by", "created_on", "data_type", "text", "updated_on" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8243), 2, "Hardcover", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8248) },
                    { 2, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8251), 2, "Trade Paperback", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8251) },
                    { 3, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8253), 2, "Mass Market Paperback", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8253) },
                    { 4, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8254), 1, "New", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8255) },
                    { 5, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8256), 1, "Like New", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8257) },
                    { 6, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8258), 1, "Good", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8259) },
                    { 7, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8260), 1, "Acceptable", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8261) },
                    { 8, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8262), 3, "Biographies", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8263) },
                    { 9, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8264), 3, "Children's Books", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8265) },
                    { 10, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8266), 3, "History", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8267) },
                    { 11, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8268), 3, "Literature & Fiction", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8269) },
                    { 12, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8270), 3, "Mystery, Thriller & Suspense", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8270) },
                    { 13, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8272), 3, "Science Fiction & Fantasy", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8272) },
                    { 14, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8273), 3, "Travel", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8274) },
                    { 15, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8275), 0, "Arcadia Books", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8276) },
                    { 16, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8277), 0, "Astral Publishing", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8278) },
                    { 17, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8279), 0, "Moonlight Publishing", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8279) },
                    { 18, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8281), 0, "Dreamscape Press", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8281) },
                    { 19, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8282), 0, "Enchanted Library", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8283) },
                    { 20, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8284), 0, "Fantasia House", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8285) },
                    { 21, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8286), 0, "Horizon Books", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8287) },
                    { 22, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8288), 0, "Infinity Press", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8289) },
                    { 23, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8290), 0, "Paradigm Publishing", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8290) },
                    { 24, "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8292), 0, "Aurora Publishing", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8292) }
                });

            migrationBuilder.InsertData(
                schema: "bobsusedbookstore_dbo",
                table: "book",
                columns: new[] { "id", "author", "book_type_id", "condition_id", "cover_image_url", "created_by", "created_on", "genre_id", "isbn", "name", "price", "publisher_id", "quantity", "summary", "updated_on", "year" },
                values: new object[,]
                {
                    { 1, "Li Juan", 1, 5, "/images/coverimages/apocalypse.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8625), 13, "6556784356", "2020: The Apocalypse", 10.95m, 15, 25, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8626), null },
                    { 2, "Nikki Wolf", 1, 6, "/images/coverimages/childrenofiron.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8632), 11, "7665438976", "Children Of Iron", 13.95m, 16, 3, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8632), null },
                    { 3, "Richard Roe", 1, 5, "/images/coverimages/goldinthedark.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8634), 13, "5442280765", "Gold In The Dark", 6.50m, 17, 10, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8635), null },
                    { 4, "Pat Candella", 2, 7, "/images/coverimages/leaguesofsmoke.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8640), 11, "4556789542", "Leagues Of Smoke", 3m, 18, 1, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8641), null },
                    { 5, "Carlos Salazar", 2, 5, "/images/coverimages/alonewiththestars.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8643), 12, "4563358087", "Alone With The Stars", 15.95m, 19, 5, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8643), null },
                    { 6, "Terri Whitlock", 1, 6, "/images/coverimages/girlinthepolaroid.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8645), 12, "2354435678", "The Girl In The Polaroid", 8.25m, 20, 2, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8646), null },
                    { 7, "Mary Major", 2, 5, "/images/coverimages/1001jokes.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8648), 11, "6554789632", "1001 Jokes", 13.95m, 21, 7, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8648), null },
                    { 8, "Mateo Jackson", 3, 7, "/images/coverimages/mysearchformeaning.png", "System", new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8650), 8, "4558786554", "My Search For Meaning", 5m, 22, 15, null, new DateTime(2025, 11, 7, 1, 27, 38, 357, DateTimeKind.Utc).AddTicks(8651), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_customer_id",
                schema: "bobsusedbookstore_dbo",
                table: "address",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_book_type_id",
                schema: "bobsusedbookstore_dbo",
                table: "book",
                column: "book_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_condition_id",
                schema: "bobsusedbookstore_dbo",
                table: "book",
                column: "condition_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_genre_id",
                schema: "bobsusedbookstore_dbo",
                table: "book",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_book_publisher_id",
                schema: "bobsusedbookstore_dbo",
                table: "book",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_sub",
                schema: "bobsusedbookstore_dbo",
                table: "customer",
                column: "sub",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_offer_book_type_id",
                schema: "bobsusedbookstore_dbo",
                table: "offer",
                column: "book_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_offer_condition_id",
                schema: "bobsusedbookstore_dbo",
                table: "offer",
                column: "condition_id");

            migrationBuilder.CreateIndex(
                name: "IX_offer_customer_id",
                schema: "bobsusedbookstore_dbo",
                table: "offer",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_offer_genre_id",
                schema: "bobsusedbookstore_dbo",
                table: "offer",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_offer_publisher_id",
                schema: "bobsusedbookstore_dbo",
                table: "offer",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_address_id",
                schema: "bobsusedbookstore_dbo",
                table: "order",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_customer_id",
                schema: "bobsusedbookstore_dbo",
                table: "order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_book_id",
                schema: "bobsusedbookstore_dbo",
                table: "order_item",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_order_id",
                schema: "bobsusedbookstore_dbo",
                table: "order_item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_cart_item_book_id",
                schema: "bobsusedbookstore_dbo",
                table: "shopping_cart_item",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_cart_item_shopping_cart_id",
                schema: "bobsusedbookstore_dbo",
                table: "shopping_cart_item",
                column: "shopping_cart_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "author",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "offer",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "order_item",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "product",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "shopping_cart_item",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "order",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "book",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "shopping_cart",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "address",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "reference_data",
                schema: "bobsusedbookstore_dbo");

            migrationBuilder.DropTable(
                name: "customer",
                schema: "bobsusedbookstore_dbo");
        }
    }
}
