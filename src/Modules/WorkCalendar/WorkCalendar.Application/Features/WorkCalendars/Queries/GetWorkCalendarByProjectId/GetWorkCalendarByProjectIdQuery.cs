using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Queries.GetWorkCalendarByProjectId;

/// <summary>
/// Query to retrieve the single Work Calendar owned by a Project
/// (BR-018-001: exactly one Work Calendar per Project). Added
/// (chat, 2026-09-13) to support the UI's "does this Project already
/// have a Work Calendar?" check, which only ever knows the Project's
/// id — never the calendar's own id.
/// </summary>
public sealed record GetWorkCalendarByProjectIdQuery(Guid ProjectId) : IRequest<Result<WorkCalendarDto>>;
