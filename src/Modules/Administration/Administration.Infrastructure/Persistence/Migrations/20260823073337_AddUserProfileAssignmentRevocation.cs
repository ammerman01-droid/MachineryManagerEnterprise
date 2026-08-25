using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Administration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileAssignmentRevocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                schema: "administration",
                table: "UserProfileAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RevokedAt",
                schema: "administration",
                table: "UserProfileAssignment",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                schema: "administration",
                table: "UserProfileAssignment");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                schema: "administration",
                table: "UserProfileAssignment");
        }
    }
}
