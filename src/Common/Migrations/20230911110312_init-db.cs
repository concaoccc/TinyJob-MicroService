using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Common.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Executors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastHeartbeat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StorageAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelativePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedulers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<long>(type: "bigint", nullable: false),
                    AssemblyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Namespace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutionParams = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedulers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedulers_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchedulerId = table.Column<long>(type: "bigint", nullable: false),
                    ExecutorId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ScheduledExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Schedulers_SchedulerId",
                        column: x => x.SchedulerId,
                        principalTable: "Schedulers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Executors",
                columns: new[] { "Id", "CreateTime", "LastHeartbeat", "Name", "Status", "UpdateTime" },
                values: new object[] { 1L, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "MockedExecutor", 1, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreateTime", "Email", "Name", "Pwd", "UpdateTime" },
                values: new object[] { 1L, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "fake@localhost", "DefaultAccount", "DefaultPassword", new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "CreateTime", "Description", "Name", "OwnerId", "RelativePath", "StorageAccount", "UpdateTime", "Version" },
                values: new object[] { 1L, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "MoeckedPackage", "MoeckedPackage", 1L, "MoeckedRelativePath", "MoeckedStorageAccount", new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0.0.1" });

            migrationBuilder.InsertData(
                table: "Schedulers",
                columns: new[] { "Id", "AssemblyName", "ClassName", "CreateTime", "ExecutionParams", "ExecutionPlan", "Name", "Namespace", "NextExecutionTime", "PackageId", "Type", "UpdateTime" },
                values: new object[] { 1L, "TestAssemblyName", "TestClassName", new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "0/30 * * * * ? *", "TestScheduler", "TestNamespace", new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, 1, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "ActualExecutionTime", "CreateTime", "ExecutorId", "FinishTime", "Name", "ScheduledExecutionTime", "SchedulerId", "Status", "UpdateTime" },
                values: new object[] { 1L, null, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "TestJob", new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, 3, new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Executors_LastHeartbeat",
                table: "Executors",
                column: "LastHeartbeat");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_SchedulerId",
                table: "Jobs",
                column: "SchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_OwnerId_Name_Version",
                table: "Packages",
                columns: new[] { "OwnerId", "Name", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_NextExecutionTime",
                table: "Schedulers",
                column: "NextExecutionTime");

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_PackageId_Name",
                table: "Schedulers",
                columns: new[] { "PackageId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Executors");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Schedulers");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
