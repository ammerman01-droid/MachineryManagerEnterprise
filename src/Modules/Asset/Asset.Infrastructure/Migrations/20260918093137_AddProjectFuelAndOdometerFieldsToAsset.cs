using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectFuelAndOdometerFieldsToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OdometerUnitOfMeasurementId",
                schema: "asset",
                table: "Asset",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryFuelKind",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                schema: "asset",
                table: "Asset",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryFuelKind",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SecondaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asset_OdometerUnitOfMeasurementId",
                schema: "asset",
                table: "Asset",
                column: "OdometerUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_PrimaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset",
                column: "PrimaryFuelUnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_ProjectId",
                schema: "asset",
                table: "Asset",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_SecondaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset",
                column: "SecondaryFuelUnitOfMeasurementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Asset_OdometerUnitOfMeasurementId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropIndex(
                name: "IX_Asset_PrimaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropIndex(
                name: "IX_Asset_ProjectId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropIndex(
                name: "IX_Asset_SecondaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "OdometerUnitOfMeasurementId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "PrimaryFuelKind",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "PrimaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "SecondaryFuelKind",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "SecondaryFuelUnitOfMeasurementId",
                schema: "asset",
                table: "Asset");
        }
    }
}
