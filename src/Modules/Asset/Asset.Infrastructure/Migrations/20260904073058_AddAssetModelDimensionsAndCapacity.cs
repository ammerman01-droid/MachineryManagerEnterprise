using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetModelDimensionsAndCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HeightValue",
                schema: "asset",
                table: "AssetModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LengthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LengthValue",
                schema: "asset",
                table: "AssetModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightValue",
                schema: "asset",
                table: "AssetModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WidthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WidthValue",
                schema: "asset",
                table: "AssetModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkingCapacityVolumeUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingCapacityVolumeValue",
                schema: "asset",
                table: "AssetModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkingCapacityWeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WorkingCapacityWeightValue",
                schema: "asset",
                table: "AssetModel",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_HeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel",
                column: "HeightUnitOfMeasurementId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetModel_HeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_LengthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_WeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_WidthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_WorkingCapacityVolumeUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_WorkingCapacityWeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "HeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "HeightValue",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "LengthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "LengthValue",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WeightValue",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WidthUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WidthValue",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WorkingCapacityVolumeUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WorkingCapacityVolumeValue",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WorkingCapacityWeightUnitOfMeasurementId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "WorkingCapacityWeightValue",
                schema: "asset",
                table: "AssetModel");
        }
    }
}
