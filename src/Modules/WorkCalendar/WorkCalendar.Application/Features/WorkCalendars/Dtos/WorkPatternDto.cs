namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.WorkPattern"/>.</summary>
public sealed record WorkPatternDto(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    IReadOnlyDictionary<DayOfWeek, DayScheduleDto> WeeklySchedule);