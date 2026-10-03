using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Usage module (Modular Monolith —
/// each module owns its own DbContext and schema, per ADR-0006). Also
/// serves as this module's <see cref="IUsageUnitOfWork"/> implementation
/// — mirrors <c>AssetDbContext</c>, including the fix (chat, 2026-08-27,
/// discovered on Asset) of registering the module-specific
/// <see cref="IUsageUnitOfWork"/> rather than the shared
/// <see cref="IUnitOfWork"/> directly, which would collide with every
/// other module's DbContext registration.
/// </summary>
public sealed class UsageDbContext : DbContext, IUsageUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="UsageDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">
    /// Optional dispatcher for publishing domain events after successful
    /// commit. Absent during design-time migrations or test isolation.
    /// </param>
    public UsageDbContext(
        DbContextOptions<UsageDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Meter Device aggregates.</summary>
    public DbSet<global::Usage.Domain.MeterDevice> MeterDevices => Set<global::Usage.Domain.MeterDevice>();

    /// <summary>Gets the set of Usage Ledger aggregates.</summary>
    public DbSet<global::Usage.Domain.UsageLedger> UsageLedgers => Set<global::Usage.Domain.UsageLedger>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// This module does NOT own the physical table — Administration does
    /// (mirrors Asset.Infrastructure's identical mapping).
    /// </summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("usage");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsageDbContext).Assembly);

        // Shared audit table: mapped for the AuditSaveChangesInterceptor
        // to write into, but excluded from this module's migrations —
        // the table is owned by Administration (mirrors Asset).
        modelBuilder.ApplyAuditEntryMapping(ownsTable: false);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Persists all pending changes for this module as a single atomic
    /// unit (ADR-0006). Domain Events raised by tracked aggregates are
    /// dispatched after a successful commit and then cleared.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
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
