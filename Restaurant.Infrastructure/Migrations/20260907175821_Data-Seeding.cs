using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "IsActive", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "شارع الجمهورية", true, "فرع المنصورة", "01000000001" },
                    { 2, "مدينة نصر", true, "فرع القاهرة", "01000000002" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "مقبلات" },
                    { 2, "أطباق رئيسية" },
                    { 3, "مشروبات" }
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "BranchId", "ItemName", "MinimumThreshold", "QuantityAvailable", "Unit" },
                values: new object[,]
                {
                    { 1, 1, "لحمة بقري", 10m, 50m, "kg" },
                    { 2, 1, "بطاطس", 20m, 100m, "kg" },
                    { 3, 1, "زيت", 5m, 30m, "liter" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "BranchId", "CategoryId", "Description", "ImageUrl", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, 1, "بطاطس مقرمشة", "", true, "بطاطس محمرة", 45m },
                    { 2, 1, 2, "برجر 200 جرام", "", true, "برجر لحمة", 120m },
                    { 3, 1, 3, "طازة", "", true, "عصير مانجة", 35m }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "BranchId", "Capacity", "TableNumber" },
                values: new object[,]
                {
                    { 1, 1, 4, "T1" },
                    { 2, 1, 2, "T2" },
                    { 3, 1, 6, "T3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
