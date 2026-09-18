using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RetireWorkPattern;

/// <summary>Command to manually retire an Active Work Pattern directly to Historical.</summary>
public sealed record RetireWorkPatternCommand(Guid WorkCalendarId, Guid WorkPatternId) : IRequest<Result>;