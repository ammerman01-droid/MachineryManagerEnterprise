using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.SharedKernel.Infrastructure;

namespace MachineryManager.Configuration.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Configuration module (Color,
/// UnitCategory, UnitOfMeasurement). Mirrors AssetDbContext's structure
/// exactly (ADR-0006 — one DbContext per module).
/// </summary>
public sealed class ConfigurationDbContext : DbContext, IConfigurationUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">Optional dispatcher for publishing domain events after successful commit.</param>
    public ConfigurationDbContext(
        DbContextOptions<ConfigurationDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Color aggregates.</summary>
    public DbSet<global::Configuration.Domain.Color> Colors => Set<global::Configuration.Domain.Color>();

    /// <summary>Gets the set of UnitOfMeasurement aggregates.</summary>
    public DbSet<global::Configuration.Domain.UnitOfMeasurement> UnitsOfMeasurement => Set<global::Configuration.Domain.UnitOfMeasurement>();

    /// <summary>Gets the set of Company aggregates.</summary>
    public DbSet<global::Configuration.Domain.Company> Companies =>
        Set<global::Configuration.Domain.Company>();

    /// <summary>Gets the set of FuelTypes aggregates.</summary>
    public DbSet<global::Configuration.Domain.FuelType> FuelTypes => Set<global::Configuration.Domain.FuelType>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// This module does NOT own the physical table — Administration does
    /// (chat, 2026-09-05, gam 3).
    /// </summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("configuration");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfigurationDbContext).Assembly);

        // Shared audit table (chat, 2026-09-05, gam 3): mapped for the
        // AuditSaveChangesInterceptor to write into, but excluded from
        // this module's migrations — the table is owned by Administration.
        modelBuilder.ApplyAuditEntryMapping(ownsTable: false);

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

        var domainEvents = aggregatesWithEvents.SelectMany(a => a.DomainEvents).ToList();
        var affectedRows = await base.SaveChangesAsync(cancellationToken);

        if (_domainEventDispatcher is not null && domainEvents.Count > 0)
            await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);

        foreach (var aggregate in aggregatesWithEvents)
            aggregate.ClearDomainEvents();

        return affectedRows;
    }
}