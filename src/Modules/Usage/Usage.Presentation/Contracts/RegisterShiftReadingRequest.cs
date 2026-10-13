namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for registering a reading for one Work Calendar shift on a Meter Device's owner (chat, 2026-09-29).</summary>
/// <param name="EntryDate">The calendar date of the shift being reported.</param>
/// <param name="ShiftIndex">The Work Calendar shift index being reported.</param>
/// <param name="RawReadingValue">The raw value shown on the device at the end of this shift.</param>
/// <param name="OperatorId">The operator this shift is attributed to, when supplied.</param>
/// <param name="ShiftStartTime">The shift's start time to record — the Work Calendar's scheduled time, or a user override (chat, 2026-10-03).</param>
/// <param name="ShiftEndTime">The shift's end time to record — same override behavior as <paramref name="ShiftStartTime"/>.</param>
public sealed record RegisterShiftReadingRequest(
    DateOnly EntryDate,
    int ShiftIndex,
    decimal RawReadingValue,
    Guid? OperatorId,
    TimeOnly ShiftStartTime,
    TimeOnly ShiftEndTime);
