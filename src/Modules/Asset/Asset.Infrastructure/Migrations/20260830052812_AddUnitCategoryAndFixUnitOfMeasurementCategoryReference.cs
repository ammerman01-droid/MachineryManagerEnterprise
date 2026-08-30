using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitCategoryAndFixUnitOfMeasurementCategoryReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_OrganizationId_Category",
                schema: "asset",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "Category",
                schema: "asset",
                table: "UnitOfMeasurement");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UnitCategory",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_OrganizationId_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement",
                columns: new[] { "OrganizationId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_UnitCategory_OrganizationId",
                schema: "asset",
                table: "UnitCategory",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitOfMeasurement_UnitCategory_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement",
                column: "CategoryId",
                principalSchema: "asset",
                principalTable: "UnitCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitOfMeasurement_UnitCategory_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement");

            migrationBuilder.DropTable(
                name: "UnitCategory",
                schema: "asset");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_OrganizationId_CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "asset",
                table: "UnitOfMeasurement");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "asset",
                table: "UnitOfMeasurement",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_OrganizationId_Category",
                schema: "asset",
                table: "UnitOfMeasurement",
                columns: new[] { "OrganizationId", "Category" });
        }
    }
}
