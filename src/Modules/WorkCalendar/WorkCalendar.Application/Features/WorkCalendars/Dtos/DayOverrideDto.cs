namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.DayOverride"/>.</summary>
public sealed record DayOverrideDto(
    Guid Id, DateOnly Date, string Type, DayScheduleDto? CustomSchedule, bool IsCancelled);