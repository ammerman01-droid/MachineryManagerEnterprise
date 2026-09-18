namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.DayResolution"/>.</summary>
public sealed record DayResolutionDto(
    DateOnly Date, DayScheduleDto Schedule, string Source, string? OverrideType);