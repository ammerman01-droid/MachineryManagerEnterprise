using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.AddWorkPattern;

/// <summary>Command to add a new Work Pattern to a Work Calendar.</summary>
/// <param name="WorkCalendarId">The calendar to add the pattern to.</param>
/// <param name="StartDate">The first date this pattern applies to (inclusive).</param>
/// <param name="EndDate">The last date this pattern applies to (inclusive).</param>
/// <param name="WeeklySchedule">The template schedule for every day of the week — all seven days must be present.</param>
public sealed record AddWorkPatternCommand(
    Guid WorkCalendarId,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyDictionary<DayOfWeek, global::WorkCalendar.Domain.DaySchedule> WeeklySchedule)
    : IRequest<Result<Guid>>;