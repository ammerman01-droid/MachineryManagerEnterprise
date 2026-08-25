using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Administration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserProfileSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                schema: "administration",
                table: "UserProfileAssignment");

            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                schema: "administration",
                table: "UserProfileAssignment",
                newName: "LastChangedAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "administration",
                table: "UserProfileAssignment",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "administration",
                table: "UserProfileAssignment");

            migrationBuilder.RenameColumn(
                name: "LastChangedAt",
                schema: "administration",
                table: "UserProfileAssignment",
                newName: "RevokedAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                schema: "administration",
                table: "UserProfileAssignment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
