using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.SupersedeWorkPattern;

/// <summary>Command to replace an Active Work Pattern with a new version (BR-018-010).</summary>
/// <param name="WorkCalendarId">The owning calendar.</param>
/// <param name="WorkPatternId">The identifier of the Active pattern being replaced.</param>
/// <param name="StartDate">The first date the new version applies to (inclusive).</param>
/// <param name="EndDate">The last date the new version applies to (inclusive).</param>
/// <param name="WeeklySchedule">The new version's template schedule — all seven days must be present.</param>
public sealed record SupersedeWorkPatternCommand(
    Guid WorkCalendarId,
    Guid WorkPatternId,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyDictionary<DayOfWeek, global::WorkCalendar.Domain.DaySchedule> WeeklySchedule)
    : IRequest<Result<Guid>>;