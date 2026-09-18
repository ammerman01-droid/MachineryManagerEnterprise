namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>The aggregate working-capacity result for a date range.</summary>
public sealed record WorkingCapacityDto(
    DateOnly StartDate, DateOnly EndDate, TimeSpan TotalNetHours, IReadOnlyList<DayResolutionDto> Days);