using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Maintenance module (Modular
/// Monolith — each module owns its own DbContext and schema, per
/// ADR-0006). Also serves as this module's <see cref="IMaintenanceUnitOfWork"/>
/// implementation, mirroring AssetDbContext/PersonnelDbContext.
/// </summary>
public sealed class MaintenanceDbContext : DbContext, IMaintenanceUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="MaintenanceDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">
    /// Optional dispatcher for publishing domain events after successful
    /// commit. Absent during design-time migrations or test isolation.
    /// </param>
    public MaintenanceDbContext(
        DbContextOptions<MaintenanceDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Work Order aggregates.</summary>
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

    /// <summary>Gets the set of per-Organization Work Order number counters.</summary>
    public DbSet<WorkOrderSequence> WorkOrderSequences => Set<WorkOrderSequence>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// This module does NOT own the physical table — Administration
    /// does, mirroring Asset/Personnel.
    /// </summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("maintenance");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaintenanceDbContext).Assembly);
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
