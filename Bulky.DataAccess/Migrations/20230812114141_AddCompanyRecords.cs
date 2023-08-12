using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBookWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2023, 8, 12, 17, 11, 41, 654, DateTimeKind.Local).AddTicks(8511));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2023, 8, 12, 17, 11, 41, 654, DateTimeKind.Local).AddTicks(8524));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2023, 8, 12, 17, 11, 41, 654, DateTimeKind.Local).AddTicks(8525));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "Pune", "Google", "1234567890", "411048", "Maharashtra", "123 Tech" },
                    { 2, "Pune", "Microsoft", "7484744677", "411048", "Maharashtra", "890 Tech" },
                    { 3, "Pune", "Amazon", "1234567890", "411048", "Maharashtra", "456 Tech" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2023, 8, 12, 16, 28, 20, 450, DateTimeKind.Local).AddTicks(2615));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2023, 8, 12, 16, 28, 20, 450, DateTimeKind.Local).AddTicks(2625));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2023, 8, 12, 16, 28, 20, 450, DateTimeKind.Local).AddTicks(2626));
        }
    }
}
