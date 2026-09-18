using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;

/// <summary>Query to retrieve a single Work Calendar by its identifier, including its Work Patterns and Day Overrides.</summary>
public sealed record GetWorkCalendarByIdQuery(Guid WorkCalendarId) : IRequest<Result<WorkCalendarDto>>;