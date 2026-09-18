using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.CancelDayOverride;

/// <summary>Command to cancel an existing Day Override (retained, never deleted — control returns to the governing Work Pattern).</summary>
public sealed record CancelDayOverrideCommand(Guid WorkCalendarId, Guid DayOverrideId) : IRequest<Result>;