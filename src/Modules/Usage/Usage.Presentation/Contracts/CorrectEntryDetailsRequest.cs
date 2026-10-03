namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for correcting a shift reading's operator attribution and/or shift times, without touching its reading or Operational Usage (chat, 2026-09-29).</summary>
/// <param name="OperatorId">The corrected operator attribution, or <see langword="null"/> to clear it.</param>
/// <param name="ShiftStartTime">The corrected shift start time.</param>
/// <param name="ShiftEndTime">The corrected shift end time.</param>
public sealed record CorrectEntryDetailsRequest(
    Guid? OperatorId,
    TimeOnly ShiftStartTime,
    TimeOnly ShiftEndTime);
