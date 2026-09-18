using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RegisterWorkCalendar;

/// <summary>Command to register a new Work Calendar for a Project.</summary>
public sealed record RegisterWorkCalendarCommand(Guid ProjectId, string Name) : IRequest<Result<Guid>>;