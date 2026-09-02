using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEngineModelTechnicalSpecifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CylinderCount",
                schema: "asset",
                table: "EngineModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EngineDisplacementUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EngineDisplacementValue",
                schema: "asset",
                table: "EngineModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnginePowerUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EnginePowerValue",
                schema: "asset",
                table: "EngineModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WeightUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightValue",
                schema: "asset",
                table: "EngineModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

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
                name: "IX_EngineModel_WeightUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel",
                column: "WeightUnitOfMeasurementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EngineModel_EngineDisplacementUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropIndex(
                name: "IX_EngineModel_EnginePowerUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropIndex(
                name: "IX_EngineModel_WeightUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "CylinderCount",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "EngineDisplacementUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "EngineDisplacementValue",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "EnginePowerUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "EnginePowerValue",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "WeightUnitOfMeasurementId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "WeightValue",
                schema: "asset",
                table: "EngineModel");
        }
    }
}
