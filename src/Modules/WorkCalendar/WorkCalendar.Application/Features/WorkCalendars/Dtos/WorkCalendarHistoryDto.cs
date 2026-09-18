namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Dtos;

/// <summary>The full historical audit trail for a Work Calendar — nothing is ever deleted (BR-018-010, BR-018-012).</summary>
public sealed record WorkCalendarHistoryDto(
    IReadOnlyList<WorkPatternDto> SupersededOrHistoricalPatterns, IReadOnlyList<DayOverrideDto> CancelledOverrides);