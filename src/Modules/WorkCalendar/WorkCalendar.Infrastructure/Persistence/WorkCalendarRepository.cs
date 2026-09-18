using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.WorkCalendar.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IWorkCalendarRepository"/>.</summary>
public sealed class WorkCalendarRepository : IWorkCalendarRepository
{
    private readonly WorkCalendarDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="WorkCalendarRepository"/> class.</summary>
    /// <param name="dbContext">The WorkCalendar module's persistence context.</param>
    public WorkCalendarRepository(WorkCalendarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::WorkCalendar.Domain.WorkCalendar?> GetByIdAsync(
        global::WorkCalendar.Domain.WorkCalendarId id, CancellationToken cancellationToken = default) =>
        _dbContext.WorkCalendars
            // Fix (chat, 2026-09-14): these are owned-collection
            // navigations mapped via OwnsMany(c => c.WorkPatterns, ...)
            // / OwnsMany(c => c.DayOverrides, ...) — EF names the
            // navigation after the public property, not the backing
            // field, so "_workPatterns"/"_dayOverrides" never matched
            // anything and silently produced an unfiltered/failed
            // Include (InvalidIncludePathError).
            .Include(nameof(global::WorkCalendar.Domain.WorkCalendar.WorkPatterns))
            .Include(nameof(global::WorkCalendar.Domain.WorkCalendar.DayOverrides))
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    /// <inheritdoc />
    public Task<global::WorkCalendar.Domain.WorkCalendar?> GetByProjectIdAsync(
        Guid projectId, CancellationToken cancellationToken = default) =>
        _dbContext.WorkCalendars
            .Include(nameof(global::WorkCalendar.Domain.WorkCalendar.WorkPatterns))
            .Include(nameof(global::WorkCalendar.Domain.WorkCalendar.DayOverrides))
            .FirstOrDefaultAsync(c => c.ProjectId == projectId, cancellationToken);

    /// <inheritdoc />
    public void Add(global::WorkCalendar.Domain.WorkCalendar aggregate) => _dbContext.WorkCalendars.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::WorkCalendar.Domain.WorkCalendar aggregate) => _dbContext.WorkCalendars.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::WorkCalendar.Domain.WorkCalendar aggregate) => _dbContext.WorkCalendars.Remove(aggregate);
}