using Microsoft.EntityFrameworkCore.Migrations;

namespace BobsBookstore.DataAccess.Migrations
{
    public partial class AddResaleStatusModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Resale",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResaleStatus_Id",
                table: "Resale",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResaleStatus",
                columns: table => new
                {
                    ResaleStatus_Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResaleStatus", x => x.ResaleStatus_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resale_ResaleStatus_Id",
                table: "Resale",
                column: "ResaleStatus_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resale_ResaleStatus_ResaleStatus_Id",
                table: "Resale",
                column: "ResaleStatus_Id",
                principalTable: "ResaleStatus",
                principalColumn: "ResaleStatus_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resale_ResaleStatus_ResaleStatus_Id",
                table: "Resale");

            migrationBuilder.DropTable(
                name: "ResaleStatus");

            migrationBuilder.DropIndex(
                name: "IX_Resale_ResaleStatus_Id",
                table: "Resale");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Resale");

            migrationBuilder.DropColumn(
                name: "ResaleStatus_Id",
                table: "Resale");
        }
    }
}
