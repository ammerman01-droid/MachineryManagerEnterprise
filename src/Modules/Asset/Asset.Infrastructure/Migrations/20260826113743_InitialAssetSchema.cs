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
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
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
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetModel_HoldingId",
                schema: "asset",
                table: "AssetModel",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_HoldingId",
                schema: "asset",
                table: "EngineModel",
                column: "HoldingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetModel",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "EngineModel",
                schema: "asset");
        }
    }
}
