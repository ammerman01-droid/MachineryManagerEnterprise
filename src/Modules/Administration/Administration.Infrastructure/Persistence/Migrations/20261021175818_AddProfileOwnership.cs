using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Administration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HoldingId",
                schema: "administration",
                table: "Profile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                schema: "administration",
                table: "Profile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "UX_Profile_HoldingId_System",
                schema: "administration",
                table: "Profile",
                column: "HoldingId",
                unique: true,
                filter: "[IsSystem] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Profile_HoldingId_System",
                schema: "administration",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "HoldingId",
                schema: "administration",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                schema: "administration",
                table: "Profile");
        }
    }
}
