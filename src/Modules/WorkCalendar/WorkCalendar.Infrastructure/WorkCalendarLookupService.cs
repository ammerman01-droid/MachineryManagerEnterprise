using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;

namespace MachineryManagerEnterprise.WorkCalendar.Infrastructure;

/// <summary>
/// Read-only cross-module lookup implementation for Work Calendar data.
/// </summary>
public sealed class WorkCalendarLookupService : IWorkCalendarLookupService
{
    private readonly IWorkCalendarRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkCalendarLookupService"/> class.
    /// </summary>
    /// <param name="repository">The Work Calendar repository.</param>
    public WorkCalendarLookupService(IWorkCalendarRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<WorkCalendarDayScheduleSnapshot?> ResolveDayScheduleAsync(
        Guid projectId,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        var calendar = await _repository.GetByProjectIdAsync(projectId, cancellationToken);

        if (calendar is null)
        {
            return null;
        }

        var resolution = calendar.ResolveDay(date);
        var schedule = resolution.Schedule;

        var shifts = schedule.Shifts
    .Select((shift, index) => new ShiftWindow(
        index,
        shift.StartTime,
        shift.EndTime))
    .ToList();

        return new WorkCalendarDayScheduleSnapshot(
            schedule.IsWorkingDay,
            shifts);
    }
}