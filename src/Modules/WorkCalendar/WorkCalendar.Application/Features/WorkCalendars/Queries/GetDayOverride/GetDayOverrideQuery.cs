using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetDayOverride;

/// <summary>Query to retrieve a single Day Override by its identifier, within its owning Work Calendar.</summary>
public sealed record GetDayOverrideQuery(Guid WorkCalendarId, Guid DayOverrideId) : IRequest<Result<DayOverrideDto>>;