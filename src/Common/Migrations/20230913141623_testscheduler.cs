using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Migrations
{
    /// <inheritdoc />
    public partial class testscheduler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Schedulers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "EndTime", "ExecutionPlan", "Type" },
                values: new object[] { new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "0 0 12 0 0", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Schedulers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "EndTime", "ExecutionPlan", "Type" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0/30 * * * * ? *", 1 });
        }
    }
}
