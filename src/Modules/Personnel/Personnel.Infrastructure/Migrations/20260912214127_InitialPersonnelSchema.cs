using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personnel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPersonnelSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "personnel");

            migrationBuilder.CreateTable(
                name: "Personnel",
                schema: "personnel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PersonnelCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonnelDrivingLicense",
                schema: "personnel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrivingLicenseTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PersonnelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelDrivingLicense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelDrivingLicense_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalSchema: "personnel",
                        principalTable: "Personnel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_CurrentProjectId",
                schema: "personnel",
                table: "Personnel",
                column: "CurrentProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_OrganizationId",
                schema: "personnel",
                table: "Personnel",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_OrganizationId_PersonnelCode",
                schema: "personnel",
                table: "Personnel",
                columns: new[] { "OrganizationId", "PersonnelCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelDrivingLicense_PersonnelId_DrivingLicenseTypeId",
                schema: "personnel",
                table: "PersonnelDrivingLicense",
                columns: new[] { "PersonnelId", "DrivingLicenseTypeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonnelDrivingLicense",
                schema: "personnel");

            migrationBuilder.DropTable(
                name: "Personnel",
                schema: "personnel");
        }
    }
}
