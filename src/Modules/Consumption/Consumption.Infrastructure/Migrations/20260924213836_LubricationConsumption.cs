using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consumption.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LubricationConsumption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumptionFreezeSetting",
                schema: "consumption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThresholdDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionFreezeSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LubricantOverflowReport",
                schema: "consumption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportDate = table.Column<DateOnly>(type: "date", nullable: false),
                    HourMeterReading = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LubricantOverflowReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LubricantOverflowLine",
                schema: "consumption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverflowComponentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LubricantTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountInLiters = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LubricantOverflowReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LubricantOverflowLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LubricantOverflowLine_LubricantOverflowReport_LubricantOverflowReportId",
                        column: x => x.LubricantOverflowReportId,
                        principalSchema: "consumption",
                        principalTable: "LubricantOverflowReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LubricantOverflowReportPersonnelEntry",
                schema: "consumption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonnelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    LubricantOverflowReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LubricantOverflowReportPersonnelEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LubricantOverflowReportPersonnelEntry_LubricantOverflowReport_LubricantOverflowReportId",
                        column: x => x.LubricantOverflowReportId,
                        principalSchema: "consumption",
                        principalTable: "LubricantOverflowReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionFreezeSetting_OrganizationId",
                schema: "consumption",
                table: "ConsumptionFreezeSetting",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LubricantOverflowLine_LubricantOverflowReportId",
                schema: "consumption",
                table: "LubricantOverflowLine",
                column: "LubricantOverflowReportId");

            migrationBuilder.CreateIndex(
                name: "IX_LubricantOverflowReport_AssetId",
                schema: "consumption",
                table: "LubricantOverflowReport",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LubricantOverflowReport_OrganizationId",
                schema: "consumption",
                table: "LubricantOverflowReport",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_LubricantOverflowReport_ProjectId",
                schema: "consumption",
                table: "LubricantOverflowReport",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LubricantOverflowReport_ReportDate",
                schema: "consumption",
                table: "LubricantOverflowReport",
                column: "ReportDate");

            migrationBuilder.CreateIndex(
                name: "IX_LubricantOverflowReportPersonnelEntry_LubricantOverflowReportId",
                schema: "consumption",
                table: "LubricantOverflowReportPersonnelEntry",
                column: "LubricantOverflowReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumptionFreezeSetting",
                schema: "consumption");

            migrationBuilder.DropTable(
                name: "LubricantOverflowLine",
                schema: "consumption");

            migrationBuilder.DropTable(
                name: "LubricantOverflowReportPersonnelEntry",
                schema: "consumption");

            migrationBuilder.DropTable(
                name: "LubricantOverflowReport",
                schema: "consumption");
        }
    }
}
