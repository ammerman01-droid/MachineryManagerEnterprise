namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.WorkCalendar"/>.</summary>
public sealed record WorkCalendarDto(
    Guid Id,
    Guid ProjectId,
    string Name,
    IReadOnlyList<WorkPatternDto> WorkPatterns,
    IReadOnlyList<DayOverrideDto> DayOverrides);