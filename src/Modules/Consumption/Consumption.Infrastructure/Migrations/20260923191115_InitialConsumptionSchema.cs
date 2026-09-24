using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consumption.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialConsumptionSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "consumption");

            migrationBuilder.CreateTable(
                name: "FuelConsumption",
                schema: "consumption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelSlot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FuelKind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FuelUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FuelTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPriceSnapshot = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MeterReadingUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MeterReading = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    DeliveredByPersonnelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceivedByPersonnelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelConsumption", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelConsumption_AssetId",
                schema: "consumption",
                table: "FuelConsumption",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelConsumption_AssetId_RecordedAtUtc",
                schema: "consumption",
                table: "FuelConsumption",
                columns: new[] { "AssetId", "RecordedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_FuelConsumption_FuelTypeId",
                schema: "consumption",
                table: "FuelConsumption",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelConsumption_OrganizationId",
                schema: "consumption",
                table: "FuelConsumption",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelConsumption_ProjectId",
                schema: "consumption",
                table: "FuelConsumption",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelConsumption",
                schema: "consumption");
        }
    }
}
