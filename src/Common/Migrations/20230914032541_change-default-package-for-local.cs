using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Migrations
{
    /// <inheritdoc />
    public partial class changedefaultpackageforlocal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "Name", "Version" },
                values: new object[] { "Echo helloworld", "HelloWorld", "1.0.0" });

            migrationBuilder.UpdateData(
                table: "Schedulers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AssemblyName", "ClassName", "Namespace" },
                values: new object[] { "JobExample", "HelloWorldJob", "JobExample" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Description", "Name", "Version" },
                values: new object[] { "MoeckedPackage", "MoeckedPackage", "0.0.1" });

            migrationBuilder.UpdateData(
                table: "Schedulers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "AssemblyName", "ClassName", "Namespace" },
                values: new object[] { "TestAssemblyName", "TestClassName", "TestNamespace" });
        }
    }
}
