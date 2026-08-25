using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Administration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileAssignmentProfileForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserProfileAssignment_ProfileId",
                schema: "administration",
                table: "UserProfileAssignment",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfileAssignment_Profile_ProfileId",
                schema: "administration",
                table: "UserProfileAssignment",
                column: "ProfileId",
                principalSchema: "administration",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfileAssignment_Profile_ProfileId",
                schema: "administration",
                table: "UserProfileAssignment");

            migrationBuilder.DropIndex(
                name: "IX_UserProfileAssignment_ProfileId",
                schema: "administration",
                table: "UserProfileAssignment");
        }
    }
}
