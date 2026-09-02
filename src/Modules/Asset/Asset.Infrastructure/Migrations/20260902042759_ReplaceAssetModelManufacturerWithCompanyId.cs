using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceAssetModelManufacturerWithCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manufacturer",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "asset",
                table: "EngineModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "asset",
                table: "AssetModel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_CompanyId",
                schema: "asset",
                table: "EngineModel",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_CompanyId",
                schema: "asset",
                table: "AssetModel",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EngineModel_CompanyId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropIndex(
                name: "IX_AssetModel_CompanyId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "asset",
                table: "EngineModel");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "asset",
                table: "AssetModel");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                schema: "asset",
                table: "EngineModel",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                schema: "asset",
                table: "AssetModel",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
