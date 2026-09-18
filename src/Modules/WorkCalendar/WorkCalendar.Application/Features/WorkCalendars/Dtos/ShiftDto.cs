namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.Shift"/>.</summary>
public sealed record ShiftDto(TimeOnly StartTime, TimeOnly EndTime, IReadOnlyList<BreakDto> Breaks);