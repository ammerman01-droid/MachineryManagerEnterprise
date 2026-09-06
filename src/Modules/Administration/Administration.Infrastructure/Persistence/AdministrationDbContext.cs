using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using MachineryManager.SharedKernel.Infrastructure;

namespace MachineryManager.Administration.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Administration module (ADR-0006).
/// Owns the "administration" schema exclusively.
/// </summary>
/// <remarks>
/// Also the designated schema OWNER of the shared
/// <c>audit.AuditEntry</c> table (chat, 2026-09-05, gam 3): this is the
/// only one of the five modules' DbContexts whose migration actually
/// generates <c>CreateTable</c> for it — every other module
/// (Organization, Asset, Configuration) maps the same entity with
/// <c>ExcludeFromMigrations()</c> so their own migrations never attempt
/// to create or alter it. Identity does not map or write to this table
/// at all (product owner, chat, 2026-09-05 — no information from the
/// Identity schema is ever recorded to the audit log).
/// </remarks>
public sealed class AdministrationDbContext : DbContext, IAdministrationUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="AdministrationDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">Optional dispatcher for publishing domain events after commit.</param>
    public AdministrationDbContext(
        DbContextOptions<AdministrationDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Profile aggregates.</summary>
    public DbSet<global::Administration.Domain.Profile> Profiles =>
        Set<global::Administration.Domain.Profile>();

    /// <summary>Gets the set of UserProfileAssignment aggregates.</summary>
    public DbSet<global::Administration.Domain.UserProfileAssignment> UserProfileAssignments =>
        Set<global::Administration.Domain.UserProfileAssignment>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// This module IS the designated owner of the physical
    /// audit.AuditEntry table (chat, 2026-09-05, gam 3).
    /// </summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("administration");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AdministrationDbContext).Assembly);

        // Shared audit table (chat, 2026-09-05, gam 3): this module is
        // the SINGLE owner of the physical audit.AuditEntry table — its
        // migration is the only one that generates CreateTable for it.
        // ownsTable: true therefore does NOT add ExcludeFromMigrations.
        modelBuilder.ApplyAuditEntryMapping(ownsTable: true);

        modelBuilder.Entity<AuditEntry>(entity =>
        {
            // No ExcludeFromMigrations here — this module's migration is
            // the one that physically creates audit.AuditEntry
            // (chat, 2026-09-05, gam 3).
            entity.ToTable("AuditEntry", "audit");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SchemaName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TableName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.RecordId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ChangesJson).IsRequired();
            entity.HasIndex(e => e.HoldingId);
            entity.HasIndex(e => e.OrganizationId);
            entity.HasIndex(e => e.OccurredAt);
        });

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregatesWithEvents = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHasDomainEvents)
            .Select(e => (IHasDomainEvents)e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = aggregatesWithEvents
            .SelectMany(a => a.DomainEvents)
            .ToList();

        var affectedRows = await base.SaveChangesAsync(cancellationToken);

        if (_domainEventDispatcher is not null && domainEvents.Count > 0)
        {
            await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
        }

        foreach (var aggregate in aggregatesWithEvents)
        {
            aggregate.ClearDomainEvents();
        }

        return affectedRows;
    }
}