using MachineryManagerEnterprise.Asset.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asset.Infrastructure.Migrations
{
    /// <summary>
    /// Data-only migration (chat, 2026-09-20): the Asset status is stored by name, so
    /// the column itself does not change — only the stored values are mapped from the
    /// old lifecycle (Draft/Registered/Commissioned/Operational/Inactive/Retired/Disposed)
    /// to the new statuses (Active/Ready/OutOfService/OutOfFleet). Written by hand
    /// because there is no model change for EF to scaffold.
    /// </summary>
    [DbContext(typeof(AssetDbContext))]
    [Migration("20260920235900_MapAssetStatusToFleetStatuses")]
    public partial class MapAssetStatusToFleetStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE [asset].[Asset]
SET [Status] = CASE [Status]
    WHEN 'Operational'  THEN 'Active'
    WHEN 'Registered'   THEN 'Ready'
    WHEN 'Commissioned' THEN 'Ready'
    WHEN 'Draft'        THEN 'Ready'
    WHEN 'Inactive'     THEN 'OutOfService'
    WHEN 'Retired'      THEN 'OutOfFleet'
    WHEN 'Disposed'     THEN 'OutOfFleet'
    ELSE [Status]
END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Best effort: the mapping is lossy (Ready could have been Registered or
            // Commissioned; OutOfFleet could have been Retired or Disposed).
            migrationBuilder.Sql(@"
UPDATE [asset].[Asset]
SET [Status] = CASE [Status]
    WHEN 'Active'       THEN 'Operational'
    WHEN 'Ready'        THEN 'Commissioned'
    WHEN 'OutOfService' THEN 'Inactive'
    WHEN 'OutOfFleet'   THEN 'Retired'
    ELSE [Status]
END;");
        }
    }
}
