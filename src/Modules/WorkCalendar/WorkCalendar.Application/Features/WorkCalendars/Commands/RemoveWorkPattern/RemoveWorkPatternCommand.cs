using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RemoveWorkPattern;

/// <summary>Command to remove a Draft Work Pattern from a Work Calendar (only Draft patterns may be removed).</summary>
public sealed record RemoveWorkPatternCommand(Guid WorkCalendarId, Guid WorkPatternId) : IRequest<Result>;