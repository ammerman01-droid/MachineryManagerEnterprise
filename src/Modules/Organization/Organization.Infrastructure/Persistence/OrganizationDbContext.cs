using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using MachineryManager.SharedKernel.Infrastructure;

namespace MachineryManager.Organization.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Organization module (ADR-0006,
/// ADR-0019 Hybrid Persistence Strategy). Owns the "organization" schema
/// exclusively; per the Modular Monolith Rules (06-development, Section
/// 6.1) no other module may reference these tables directly. Also
/// serves as this module's <see cref="IOrganizationUnitOfWork"/>
/// implementation (chat, 2026-08-27 — was the shared
/// <see cref="IUnitOfWork"/> directly, which caused a DI registration
/// collision with other modules' DbContexts).
/// </summary>
public sealed class OrganizationDbContext : DbContext, IOrganizationUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="OrganizationDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">
    /// Optional dispatcher for publishing domain events after successful
    /// commit. Absent during design-time migrations or test isolation.
    /// </param>
    public OrganizationDbContext(
        DbContextOptions<OrganizationDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Organization aggregates.</summary>
    public DbSet<global::Organization.Domain.Organization> Organizations =>
        Set<global::Organization.Domain.Organization>();

    /// <summary>Gets the set of Holding aggregates (BR-017, chat 2026-08-19).</summary>
    public DbSet<global::Organization.Domain.Holding> Holdings =>
        Set<global::Organization.Domain.Holding>();

    /// <summary>Gets the set of Project aggregates (BR-017, chat 2026-08-19).</summary>
    public DbSet<global::Organization.Domain.Project> Projects =>
        Set<global::Organization.Domain.Project>();

    /// <summary>
    /// Gets the set of audit records captured for this module's schema.
    /// This module does NOT own the physical table — Administration does.
    /// </summary>
    public DbSet<MachineryManager.SharedKernel.AuditEntry> AuditEntries =>
        Set<MachineryManager.SharedKernel.AuditEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("organization");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrganizationDbContext).Assembly);

        // Shared audit table (chat, 2026-09-05, gam 3): this context only
        // reads/writes rows. The physical table is created exclusively by
        // AdministrationDbContext's migration — the ONLY owner. With
        // ownsTable: false, ExcludeFromMigrations ensures `dotnet ef
        // migrations add` for this module never emits CreateTable/
        // AlterTable for AuditEntry.
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