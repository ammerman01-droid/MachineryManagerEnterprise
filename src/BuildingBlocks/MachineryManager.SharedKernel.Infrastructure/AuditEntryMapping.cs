using MachineryManager.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.SharedKernel.Infrastructure;

/// <summary>
/// Shared EF Core mapping for the platform-wide <see cref="AuditEntry"/>
/// table (chat, 2026-09-05, gam 3) — single source of truth so the
/// mapping can never drift between modules.
/// </summary>
/// <remarks>
/// <para>
/// Must be applied in EVERY DbContext that has
/// <see cref="AuditSaveChangesInterceptor"/> attached, because the
/// interceptor calls <c>context.Set&lt;AuditEntry&gt;()</c>, which
/// requires the entity to be part of the model.
/// </para>
/// <para>
/// The physical table is created by exactly ONE migration — the
/// Administration module's (<c>AdministrationDbContext</c>). Every other
/// consumer passes <c>ownsTable: false</c> and receives
/// <c>ExcludeFromMigrations()</c>, guaranteeing its own migrations never
/// emit CreateTable/AlterTable for this table.
/// </para>
/// </remarks>
public static class AuditEntryMapping
{
    /// <summary>
    /// Applies the <see cref="AuditEntry"/> mapping to the shared
    /// <c>audit.AuditEntry</c> table.
    /// </summary>
    /// <param name="modelBuilder">The model builder of the consuming DbContext.</param>
    /// <param name="ownsTable">
    /// <c>true</c> only for the single module whose migration physically
    /// creates the table (Administration). <c>false</c> for every other
    /// module — adds <c>ExcludeFromMigrations()</c> to the table mapping.
    /// </param>
    public static void ApplyAuditEntryMapping(this ModelBuilder modelBuilder, bool ownsTable)
    {
        modelBuilder.Entity<AuditEntry>(entity =>
        {
            entity.ToTable("AuditEntry", "audit", tableBuilder =>
            {
                if (!ownsTable)
                {
                    // Excluded tables never appear in this module's
                    // migrations — no manual cleanup of generated
                    // migration files is needed now or in the future.
                    tableBuilder.ExcludeFromMigrations();
                }
            });

            entity.HasKey(e => e.Id);
            entity.Property(e => e.SchemaName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TableName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.RecordId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ChangesJson).IsRequired();
            entity.HasIndex(e => e.HoldingId);
            entity.HasIndex(e => e.OrganizationId);
            entity.HasIndex(e => e.OccurredAt);
        });
    }
}