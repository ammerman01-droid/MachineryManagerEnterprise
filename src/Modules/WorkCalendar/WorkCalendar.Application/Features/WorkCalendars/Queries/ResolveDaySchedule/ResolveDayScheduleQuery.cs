using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.ResolveDaySchedule;

/// <summary>Query to resolve the effective schedule for a specific calendar date (Day Override, if any, else the governing Work Pattern).</summary>
public sealed record ResolveDayScheduleQuery(Guid WorkCalendarId, DateOnly Date) : IRequest<Result<DayResolutionDto>>;