using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarHistory;

/// <summary>Query to retrieve every non-active historical record for a Work Calendar (Superseded/Historical Work Patterns, Cancelled Day Overrides) — nothing is ever deleted, so this is the full audit trail.</summary>
public sealed record GetWorkCalendarHistoryQuery(Guid WorkCalendarId) : IRequest<Result<WorkCalendarHistoryDto>>;