namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>Read-only projection of a <see cref="global::WorkCalendar.Domain.DaySchedule"/>.</summary>
public sealed record DayScheduleDto(bool IsWorkingDay, IReadOnlyList<ShiftDto> Shifts);