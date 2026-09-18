using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetActiveWorkPatternForDate;

/// <summary>Query to retrieve whichever Work Pattern governs a specific calendar date (BR-018-003: exactly one, if any).</summary>
public sealed record GetActiveWorkPatternForDateQuery(Guid WorkCalendarId, DateOnly Date) : IRequest<Result<WorkPatternDto?>>;