using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnitCategoryAndAddKindToUnitOfMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitOfMeasurement_UnitCategory_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement");

            migrationBuilder.DropTable(
                name: "UnitCategory",
                schema: "configuration");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement");

            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_HoldingId_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement");

            migrationBuilder.AddColumn<string>(
                name: "Kind",
                schema: "configuration",
                table: "UnitOfMeasurement",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_HoldingId_Kind",
                schema: "configuration",
                table: "UnitOfMeasurement",
                columns: new[] { "HoldingId", "Kind" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UnitOfMeasurement_HoldingId_Kind",
                schema: "configuration",
                table: "UnitOfMeasurement");

            migrationBuilder.DropColumn(
                name: "Kind",
                schema: "configuration",
                table: "UnitOfMeasurement");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UnitCategory",
                schema: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_HoldingId_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                columns: new[] { "HoldingId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_UnitCategory_HoldingId",
                schema: "configuration",
                table: "UnitCategory",
                column: "HoldingId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitOfMeasurement_UnitCategory_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                column: "CategoryId",
                principalSchema: "configuration",
                principalTable: "UnitCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
