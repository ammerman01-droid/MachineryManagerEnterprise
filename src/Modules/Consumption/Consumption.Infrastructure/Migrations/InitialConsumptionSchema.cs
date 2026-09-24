// NOTE (chat, 2026-09-20): This file is a hand-written REFERENCE for what
// `dotnet ef migrations add InitialConsumptionSchema --project Consumption.Infrastructure
// --startup-project Host` should produce, based on FuelConsumptionConfiguration.cs.
// It intentionally has NO accompanying .Designer.cs or updated
// ConsumptionDbContextModelSnapshot.cs — those are large, EF-tooling-generated
// artifacts that must stay byte-consistent with the exact provider/EF Core
// version in use; hand-writing them risks a snapshot mismatch that breaks
// every migration after it. Please run the real `dotnet ef migrations add`
// command instead of pasting this file in as-is, and use this only to sanity
// check the generated Up()/Down() bodies.
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachineryManagerEnterprise.Consumption.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialConsumptionSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "consumption");

        migrationBuilder.CreateTable(
            name: "FuelConsumption",
            schema: "consumption",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                AssetId = table.Column<Guid>(nullable: false),
                OrganizationId = table.Column<Guid>(nullable: false),
                ProjectId = table.Column<Guid>(nullable: false),
                FuelSlot = table.Column<string>(maxLength: 20, nullable: false),
                FuelKind = table.Column<string>(maxLength: 20, nullable: false),
                FuelUnit = table.Column<string>(maxLength: 20, nullable: false),
                MeterReadingUnit = table.Column<string>(maxLength: 20, nullable: false),
                UnitPriceSnapshot = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                MeterReading = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                DeliveredByPersonnelId = table.Column<Guid>(nullable: false),
                ReceivedByPersonnelId = table.Column<Guid>(nullable: false),
                RecordedAtUtc = table.Column<DateTimeOffset>(nullable: false),
                Notes = table.Column<string>(maxLength: 500, nullable: true)
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
            name: "IX_FuelConsumption_OrganizationId",
            schema: "consumption",
            table: "FuelConsumption",
            column: "OrganizationId");

        migrationBuilder.CreateIndex(
            name: "IX_FuelConsumption_ProjectId",
            schema: "consumption",
            table: "FuelConsumption",
            column: "ProjectId");

        // AuditEntry is mapped here with ownsTable:false (Administration owns
        // the physical table) — mirrors AssetDbContext, so no CreateTable for
        // it in this module's migration.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FuelConsumption",
            schema: "consumption");
    }
}
