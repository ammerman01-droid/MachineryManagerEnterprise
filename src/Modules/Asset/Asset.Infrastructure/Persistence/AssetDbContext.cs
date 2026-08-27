using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Asset.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Asset module (Modular Monolith —
/// each module owns its own DbContext and schema, per ADR-0006).
/// </summary>
public sealed class AssetDbContext : DbContext, IUnitOfWork
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

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("asset");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);

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