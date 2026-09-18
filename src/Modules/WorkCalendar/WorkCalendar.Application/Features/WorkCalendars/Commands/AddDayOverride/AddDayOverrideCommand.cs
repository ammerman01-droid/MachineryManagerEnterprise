using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.AddDayOverride;

/// <summary>Command to add an explicit Day Override to a Work Calendar for a specific date.</summary>
/// <param name="WorkCalendarId">The owning calendar.</param>
/// <param name="Date">The calendar date this override applies to.</param>
/// <param name="Type">The category of this override.</param>
/// <param name="CustomSchedule">The schedule for this date — required unless <paramref name="Type"/> is Holiday.</param>
public sealed record AddDayOverrideCommand(
    Guid WorkCalendarId,
    DateOnly Date,
    global::WorkCalendar.Domain.DayOverrideType Type,
    global::WorkCalendar.Domain.DaySchedule? CustomSchedule)
    : IRequest<Result<Guid>>;