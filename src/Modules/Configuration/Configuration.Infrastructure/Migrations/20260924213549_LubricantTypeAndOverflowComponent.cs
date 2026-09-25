using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LubricantTypeAndOverflowComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LubricantType",
                schema: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LubricantType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OverflowComponent",
                schema: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverflowComponent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LubricantType_HoldingId_Name",
                schema: "configuration",
                table: "LubricantType",
                columns: new[] { "HoldingId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverflowComponent_HoldingId_Name",
                schema: "configuration",
                table: "OverflowComponent",
                columns: new[] { "HoldingId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LubricantType",
                schema: "configuration");

            migrationBuilder.DropTable(
                name: "OverflowComponent",
                schema: "configuration");
        }
    }
}
