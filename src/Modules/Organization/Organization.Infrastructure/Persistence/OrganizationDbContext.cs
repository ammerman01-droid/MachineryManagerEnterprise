using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Organization.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the Organization module (ADR-0006,
/// ADR-0019 Hybrid Persistence Strategy). Owns the "organization" schema
/// exclusively; per the Modular Monolith Rules (06-development, Section
/// 6.1) no other module may reference these tables directly. Also
/// serves as this module's <see cref="IUnitOfWork"/> implementation.
/// </summary>
public sealed class OrganizationDbContext : DbContext, IUnitOfWork
{
    /// <summary>Initializes a new instance of the <see cref="OrganizationDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options)
        : base(options)
    {
    }

    /// <summary>Gets the set of Organization aggregates.</summary>
    public DbSet<global::Organization.Domain.Organization> Organizations =>
        Set<global::Organization.Domain.Organization>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("organization");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrganizationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Persists all pending changes for this module as a single atomic
    /// unit (ADR-0006). Domain Events raised by tracked aggregates are
    /// cleared after a successful commit.
    /// </summary>
    /// <remarks>
    /// Dispatching the collected events to subscribers is NOT wired yet:
    /// no event dispatch mechanism has been approved for Infrastructure
    /// (ADR-0011 restricts MediatR to the Application layer only), and
    /// no Domain Event subscribers exist for this module yet either.
    /// This is an explicit open item — see the completion report — not
    /// a silent omission of Section 4.7's Event Publishing Rules.
    /// </remarks>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregatesWithEvents = ChangeTracker
            .Entries<global::Organization.Domain.Organization>()
            .Select(entry => entry.Entity)
            .Where(aggregate => aggregate.DomainEvents.Count > 0)
            .ToList();

        var affectedRows = await base.SaveChangesAsync(cancellationToken);

        foreach (var aggregate in aggregatesWithEvents)
        {
            aggregate.ClearDomainEvents();
        }

        return affectedRows;
    }
}
