using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Usage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialUsageSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "usage");

            migrationBuilder.CreateTable(
                name: "MeterDevice",
                schema: "usage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OwnerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DailyCapOverride = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterDevice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsageCapSetting",
                schema: "usage",
                columns: table => new
                {
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DefaultDailyCap = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageCapSetting", x => x.Unit);
                });

            migrationBuilder.CreateTable(
                name: "UsageLedger",
                schema: "usage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FrozenUpToDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageLedger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsageEntry",
                schema: "usage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EntryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ShiftIndex = table.Column<int>(type: "int", nullable: true),
                    ShiftStartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    ShiftEndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    RawReadingValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SourceMeterDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationalUsageAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OperatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsageLedgerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageEntry_UsageLedger_UsageLedgerId",
                        column: x => x.UsageLedgerId,
                        principalSchema: "usage",
                        principalTable: "UsageLedger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "usage",
                table: "UsageCapSetting",
                columns: new[] { "Unit", "DefaultDailyCap" },
                values: new object[,]
                {
                    { "Hour", 24m },
                    { "Kilometer", 1000m },
                    { "Mile", 1000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterDevice_OrganizationId",
                schema: "usage",
                table: "MeterDevice",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterDevice_OwnerType_OwnerId",
                schema: "usage",
                table: "MeterDevice",
                columns: new[] { "OwnerType", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsageEntry_ProjectId",
                schema: "usage",
                table: "UsageEntry",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageEntry_SourceMeterDeviceId",
                schema: "usage",
                table: "UsageEntry",
                column: "SourceMeterDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageEntry_UsageLedgerId_EntryDate_ShiftIndex",
                schema: "usage",
                table: "UsageEntry",
                columns: new[] { "UsageLedgerId", "EntryDate", "ShiftIndex" },
                unique: true,
                filter: "[ShiftIndex] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsageEntry_UsageLedgerId_Sequence",
                schema: "usage",
                table: "UsageEntry",
                columns: new[] { "UsageLedgerId", "Sequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsageLedger_OrganizationId",
                schema: "usage",
                table: "UsageLedger",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageLedger_OwnerType_OwnerId_Unit",
                schema: "usage",
                table: "UsageLedger",
                columns: new[] { "OwnerType", "OwnerId", "Unit" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterDevice",
                schema: "usage");

            migrationBuilder.DropTable(
                name: "UsageCapSetting",
                schema: "usage");

            migrationBuilder.DropTable(
                name: "UsageEntry",
                schema: "usage");

            migrationBuilder.DropTable(
                name: "UsageLedger",
                schema: "usage");
        }
    }
}
