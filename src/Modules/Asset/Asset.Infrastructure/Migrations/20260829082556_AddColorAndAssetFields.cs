using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColorAndAssetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "asset",
                table: "Asset");

            migrationBuilder.AddColumn<string>(
                name: "BodyNumber",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChassisNumber",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ColorId",
                schema: "asset",
                table: "Asset",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vin",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Color",
                schema: "asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.Id);
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Color_ColorId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropTable(
                name: "Color",
                schema: "asset");

            migrationBuilder.DropIndex(
                name: "IX_Asset_ColorId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "BodyNumber",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "ChassisNumber",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "ColorId",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "asset",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "Vin",
                schema: "asset",
                table: "Asset");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "asset",
                table: "Asset",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
