using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using MachineryManager.SharedKernel.Infrastructure;

namespace MachineryManager.Asset.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Asset module (Modular Monolith —
/// each module owns its own DbContext and schema, per ADR-0006). Also
/// serves as this module's <see cref="IAssetUnitOfWork"/> implementation
/// (chat, 2026-08-27 — was the shared <see cref="IUnitOfWork"/> directly,
/// which caused a DI registration collision with other modules'
/// DbContexts).
/// </summary>
public sealed class AssetDbContext : DbContext, IAssetUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="AssetDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">
    /// Optional dispatcher for publishing domain events after successful
    /// commit. Absent during design-time migrations or test isolation.
    /// </param>
    public AssetDbContext(
        DbContextOptions<AssetDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Asset aggregates.</summary>
    public DbSet<global::Asset.Domain.Asset> Assets => Set<global::Asset.Domain.Asset>();

    /// <summary>Gets the set of Asset Model aggregates.</summary>
    public DbSet<global::Asset.Domain.AssetModel> AssetModels => Set<global::Asset.Domain.AssetModel>();

    /// <summary>Gets the set of Engine Model aggregates.</summary>
    public DbSet<global::Asset.Domain.EngineModel> EngineModels => Set<global::Asset.Domain.EngineModel>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// This module does NOT own the physical table — Administration does
    /// (chat, 2026-09-05, gam 3).
    /// </summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("asset");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);

        // Shared audit table (chat, 2026-09-05, gam 3): mapped for the
        // AuditSaveChangesInterceptor to write into, but excluded from
        // this module's migrations — the table is owned by Administration.
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