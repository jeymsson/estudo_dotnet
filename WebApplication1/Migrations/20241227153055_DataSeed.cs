using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "797b41bc-3e75-4d2c-aa0d-c4a044d13c2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c677d67e-1a66-4f9b-af73-086e17c8abe3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3b287e5e-582b-4622-bfec-7ee6ced61603", null, "User", "USER" },
                    { "dcfba8f2-282b-4752-aabb-d0cd89240a9a", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "15e579c8-559e-4bbc-af94-ef8fa8b76625", "admin@email.com", false, false, null, null, "ADMIN", null, null, false, "82375435-1988-4437-88bc-80f07fb0922a", false, "admin" });

            migrationBuilder.InsertData(
                table: "Stock",
                columns: new[] { "Id", "CompanyName", "Industry", "LastDiv", "MarketCap", "Purchase", "Symbol" },
                values: new object[,]
                {
                    { 1, "Apple Inc.", "Technology", 0.82m, 2000000000L, 100m, "AAPL" },
                    { 2, "Microsoft Corporation", "Technology", 1.56m, 2000000000L, 200m, "MSFT" }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "Content", "StockId", "Title" },
                values: new object[,]
                {
                    { 1, "This is a comment", 1, "Comment 1" },
                    { 2, "This is another comment", 1, "Comment 2" }
                });

            migrationBuilder.InsertData(
                table: "Portfolios",
                columns: new[] { "AppUserId", "StockId" },
                values: new object[] { "1", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b287e5e-582b-4622-bfec-7ee6ced61603");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcfba8f2-282b-4752-aabb-d0cd89240a9a");

            migrationBuilder.DeleteData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comment",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Portfolios",
                keyColumns: new[] { "AppUserId", "StockId" },
                keyValues: new object[] { "1", 1 });

            migrationBuilder.DeleteData(
                table: "Stock",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Stock",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "797b41bc-3e75-4d2c-aa0d-c4a044d13c2a", null, "Admin", "ADMIN" },
                    { "c677d67e-1a66-4f9b-af73-086e17c8abe3", null, "User", "USER" }
                });
        }
    }
}
