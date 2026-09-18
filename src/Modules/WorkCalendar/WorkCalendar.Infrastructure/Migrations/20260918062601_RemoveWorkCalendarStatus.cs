using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkCalendar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorkCalendarStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "workcalendar",
                table: "WorkCalendar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "workcalendar",
                table: "WorkCalendar",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
