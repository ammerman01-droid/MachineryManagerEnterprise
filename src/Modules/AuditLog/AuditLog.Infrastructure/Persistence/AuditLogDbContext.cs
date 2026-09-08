using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.AuditLog.Infrastructure.Persistence;

/// <summary>
/// EF Core read context over the shared <c>audit.AuditEntry</c> table
/// (chat, 2026-09-05, gam 4 — read-only AuditLog module).
/// </summary>
/// <remarks>
/// <para>
/// STRICTLY read-only: unlike the business modules, this context is
/// registered WITHOUT <see cref="AuditSaveChangesInterceptor"/> and
/// exposes no SaveChanges overrides — the AuditLog module must never
/// write audit rows.
/// </para>
/// <para>
/// The physical table is owned by the Administration module; this
/// context maps it via <see cref="AuditEntryMapping"/> with
/// <c>ownsTable: false</c> (ExcludeFromMigrations) and generates no
/// migrations of its own.
/// </para>
/// </remarks>
public sealed class AuditLogDbContext : DbContext
{
    /// <summary>Initializes a new instance of the <see cref="AuditLogDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    public AuditLogDbContext(DbContextOptions<AuditLogDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets the shared, platform-wide set of audit records (read-only).</summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAuditEntryMapping(ownsTable: false);

        base.OnModelCreating(modelBuilder);
    }
}