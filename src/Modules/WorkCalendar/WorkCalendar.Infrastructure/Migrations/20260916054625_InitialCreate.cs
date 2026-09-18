using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkCalendar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workcalendar");

            migrationBuilder.CreateTable(
                name: "WorkCalendar",
                schema: "workcalendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCalendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayOverride",
                schema: "workcalendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomSchedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    WorkCalendarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayOverride_WorkCalendar_WorkCalendarId",
                        column: x => x.WorkCalendarId,
                        principalSchema: "workcalendar",
                        principalTable: "WorkCalendar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPattern",
                schema: "workcalendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkCalendarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeeklySchedule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPattern", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPattern_WorkCalendar_WorkCalendarId",
                        column: x => x.WorkCalendarId,
                        principalSchema: "workcalendar",
                        principalTable: "WorkCalendar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayOverride_WorkCalendarId",
                schema: "workcalendar",
                table: "DayOverride",
                column: "WorkCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCalendar_ProjectId",
                schema: "workcalendar",
                table: "WorkCalendar",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkPattern_WorkCalendarId",
                schema: "workcalendar",
                table: "WorkPattern",
                column: "WorkCalendarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayOverride",
                schema: "workcalendar");

            migrationBuilder.DropTable(
                name: "WorkPattern",
                schema: "workcalendar");

            migrationBuilder.DropTable(
                name: "WorkCalendar",
                schema: "workcalendar");
        }
    }
}
