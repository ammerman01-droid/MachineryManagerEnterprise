using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MeterDiviceNameAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "usage",
                table: "MeterDevice",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
            
                migrationBuilder.Sql(
                "UPDATE [usage].[MeterDevice] SET [Name] = N'دستگاه ' + CONVERT(nvarchar(36), [Id]) WHERE [Name] = N''");

            migrationBuilder.CreateIndex(
                name: "IX_MeterDevice_OrganizationId_Name",
                schema: "usage",
                table: "MeterDevice",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeterDevice_OrganizationId_Name",
                schema: "usage",
                table: "MeterDevice");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "usage",
                table: "MeterDevice");
        }
    }
}
