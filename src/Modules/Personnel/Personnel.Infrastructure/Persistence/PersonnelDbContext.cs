using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Personnel.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Personnel module (ADR-0006 — one
/// DbContext per module). Mirrors ConfigurationDbContext's structure.
/// </summary>
public sealed class PersonnelDbContext : DbContext, IPersonnelUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="PersonnelDbContext"/> class.</summary>
    public PersonnelDbContext(
        DbContextOptions<PersonnelDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>
    /// Gets the set of Personnel aggregates. Named "PersonnelRecords"
    /// (rather than "Personnel") to avoid a DbSet property sharing its
    /// name with the enclosing module's namespace segment.
    /// </summary>
    public DbSet<global::MachineryManagerEnterprise.Personnel.Domain.Personnel> PersonnelRecords =>
        Set<global::MachineryManagerEnterprise.Personnel.Domain.Personnel>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// ⚠️ Assumption (mirroring ConfigurationDbContext, which you
    /// confirmed does NOT own the physical table — Administration
    /// does): applying the same convention here. Confirm this is still
    /// correct for a brand-new module before running the migration.
    /// </summary>
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("personnel");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonnelDbContext).Assembly);
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