using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_IsActive_To_Color_Company_FuelType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "configuration",
                table: "FuelType",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "configuration",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "configuration",
                table: "Color",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "AssetOperationalStatus",
                schema: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetOperationalStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetOperationalStatus_HoldingId",
                schema: "configuration",
                table: "AssetOperationalStatus",
                column: "HoldingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetOperationalStatus",
                schema: "configuration");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "configuration",
                table: "FuelType");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "configuration",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "configuration",
                table: "Color");
        }
    }
}
