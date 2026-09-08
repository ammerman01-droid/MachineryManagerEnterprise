using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Administration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateSharedAuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.CreateTable(
                name: "AuditEntry",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SchemaName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecordId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    ChangesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntry", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_HoldingId",
                schema: "audit",
                table: "AuditEntry",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_OccurredAt",
                schema: "audit",
                table: "AuditEntry",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_OrganizationId",
                schema: "audit",
                table: "AuditEntry",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntry",
                schema: "audit");
        }
    }
}
