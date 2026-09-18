using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkPattern;

/// <summary>Query to retrieve a single Work Pattern by its identifier, within its owning Work Calendar.</summary>
public sealed record GetWorkPatternQuery(Guid WorkCalendarId, Guid WorkPatternId) : IRequest<Result<WorkPatternDto>>;