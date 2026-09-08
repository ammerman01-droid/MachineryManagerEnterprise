using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColorAndUnitTablesFromAssetModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Color_ColorId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropTable(
                name: "Color",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurement",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "UnitCategory",
                schema: "asset");

            migrationBuilder.DropIndex(
                name: "IX_Asset_ColorId",
                schema: "asset",
                table: "Asset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Color",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitCategory",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurement",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurement_UnitCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "asset",
                        principalTable: "UnitCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asset_ColorId",
                schema: "asset",
                table: "Asset",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Color_OrganizationId",
                schema: "asset",
                table: "Color",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitCategory_OrganizationId",
                schema: "asset",
                table: "UnitCategory",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_OrganizationId",
                schema: "asset",
                table: "UnitOfMeasurement",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_OrganizationId_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement",
                columns: new[] { "OrganizationId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Color_ColorId",
                schema: "asset",
                table: "Asset",
                column: "ColorId",
                principalSchema: "asset",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
