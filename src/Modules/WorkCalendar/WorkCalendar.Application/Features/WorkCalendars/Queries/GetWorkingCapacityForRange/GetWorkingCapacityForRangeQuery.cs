using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkingCapacityForRange;

/// <summary>Query to compute the total working capacity (BR-018-009) across a date range — the maintenance-window-planning and utilization-denominator input.</summary>
public sealed record GetWorkingCapacityForRangeQuery(Guid WorkCalendarId, DateOnly StartDate, DateOnly EndDate)
    : IRequest<Result<WorkingCapacityDto>>;