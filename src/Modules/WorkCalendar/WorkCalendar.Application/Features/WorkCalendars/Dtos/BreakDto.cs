namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.Break"/>.</summary>
public sealed record BreakDto(TimeOnly StartTime, TimeOnly EndTime);