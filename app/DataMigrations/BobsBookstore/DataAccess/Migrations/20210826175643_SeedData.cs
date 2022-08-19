using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderStatus",
                columns: new[] { "OrderStatus_Id", "Position", "Status" },
                values: new object[,]
                {
                    { 1L, 2, "Just Placed" },
                    { 2L, 3, "En Route" },
                    { 3L, 1, "Pending" },
                    { 4L, 4, "Delivered" }
                });

            migrationBuilder.InsertData(
                table: "ResaleStatus",
                columns: new[] { "ResaleStatus_Id", "Status" },
                values: new object[,]
                {
                    { 1L, "Pending Approval" },
                    { 2L, "Approved/Awaiting Shipment from Customer" },
                    { 3L, "Rejected" },
                    { 4L, "Shipment Receipt Confirmed" },
                    { 5L, "Payment Completed"}
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "OrderStatus_Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "OrderStatus_Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "OrderStatus_Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "OrderStatus",
                keyColumn: "OrderStatus_Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "ResaleStatus",
                keyColumn: "ResaleStatus_Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ResaleStatus",
                keyColumn: "ResaleStatus_Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ResaleStatus",
                keyColumn: "ResaleStatus_Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ResaleStatus",
                keyColumn: "ResaleStatus_Id",
                keyValue: 4L);
        }
    }
}
