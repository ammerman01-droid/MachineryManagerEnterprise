using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;

namespace MachineryManagerEnterprise.WorkCalendar.Infrastructure.Persistence;

/// <summary>
/// EF Core persistence context for the WorkCalendar module (Modular
/// Monolith — each module owns its own DbContext and schema, per
/// ADR-0006). Also serves as this module's <see cref="IWorkCalendarUnitOfWork"/>
/// implementation (module-specific Unit of Work — never register the
/// shared <see cref="IUnitOfWork"/> directly, per the DI-collision
/// lesson from Organization/Asset).
/// </summary>
public sealed class WorkCalendarDbContext : DbContext, MachineryManagerEnterprise.WorkCalendar.Application.Abstractions.IWorkCalendarUnitOfWork
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>Initializes a new instance of the <see cref="WorkCalendarDbContext"/> class.</summary>
    /// <param name="options">The EF Core options for this context.</param>
    /// <param name="domainEventDispatcher">
    /// Optional dispatcher for publishing domain events after successful
    /// commit. Absent during design-time migrations or test isolation.
    /// </param>
    public WorkCalendarDbContext(
        DbContextOptions<WorkCalendarDbContext> options,
        IDomainEventDispatcher? domainEventDispatcher = null)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets the set of Work Calendar aggregates.</summary>
    public DbSet<global::WorkCalendar.Domain.WorkCalendar> WorkCalendars => Set<global::WorkCalendar.Domain.WorkCalendar>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("workcalendar");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkCalendarDbContext).Assembly);

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