using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialConfigurationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "configuration");

            migrationBuilder.CreateTable(
                name: "Color",
                schema: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurement",
                schema: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurement_UnitCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "configuration",
                        principalTable: "UnitCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Color_HoldingId",
                schema: "configuration",
                table: "Color",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitCategory_HoldingId",
                schema: "configuration",
                table: "UnitCategory",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_HoldingId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurement_HoldingId_CategoryId",
                schema: "configuration",
                table: "UnitOfMeasurement",
                columns: new[] { "HoldingId", "CategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Color",
                schema: "configuration");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurement",
                schema: "configuration");

            migrationBuilder.DropTable(
                name: "UnitCategory",
                schema: "configuration");
        }
    }
}
