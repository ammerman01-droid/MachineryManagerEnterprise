using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialAssetSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset");

            migrationBuilder.CreateTable(
                name: "AssetModel",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LengthValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    LengthUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WidthValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    WidthUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HeightValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    HeightUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeightValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    WeightUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkingCapacityVolumeValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    WorkingCapacityVolumeUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkingCapacityWeightValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    WorkingCapacityWeightUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompatibleEngineModelIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EngineModel",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelKind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CylinderCount = table.Column<int>(type: "int", nullable: true),
                    EngineDisplacementValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    EngineDisplacementUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnginePowerValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    EnginePowerUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WeightValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    WeightUnitOfMeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    AssetModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationalStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChassisNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BodyNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Vin = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LicensePlate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ManufactureYear = table.Column<int>(type: "int", nullable: true),
                    MeterReadingUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PrimaryFuelKind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PrimaryFuelUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecondaryFuelKind = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecondaryFuelUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_AssetModel_AssetModelId",
                        column: x => x.AssetModelId,
                        principalSchema: "asset",
                        principalTable: "AssetModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetModelId",
                schema: "asset",
                table: "Asset",
                column: "AssetModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_OrganizationId",
                schema: "asset",
                table: "Asset",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_OrganizationId_Code",
                schema: "asset",
                table: "Asset",
                columns: new[] { "OrganizationId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asset_ProjectId",
                schema: "asset",
                table: "Asset",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_CompanyId",
                schema: "asset",
                table: "AssetModel",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_HeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "HeightUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_HoldingId",
                schema: "asset",
                table: "AssetModel",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_LengthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "LengthUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_WeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "WeightUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_WidthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "WidthUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_WorkingCapacityVolumeUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "WorkingCapacityVolumeUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_WorkingCapacityWeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "WorkingCapacityWeightUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_CompanyId",
                schema: "asset",
                table: "EngineModel",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_EngineDisplacementUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                column: "EngineDisplacementUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_EnginePowerUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                column: "EnginePowerUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_HoldingId",
                schema: "asset",
                table: "EngineModel",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_WeightUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                column: "WeightUnitOfMeasurementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asset",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "EngineModel",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "AssetModel",
                schema: "asset");
        }
    }
}
