namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for registering a reading for one Work Calendar shift on a Meter Device's owner (chat, 2026-09-29).</summary>
/// <param name="EntryDate">The calendar date of the shift being reported.</param>
/// <param name="ShiftIndex">The Work Calendar shift index being reported.</param>
/// <param name="RawReadingValue">The raw value shown on the device at the end of this shift.</param>
/// <param name="OperatorId">The operator this shift is attributed to, when supplied.</param>
public sealed record RegisterShiftReadingRequest(
    DateOnly EntryDate,
    int ShiftIndex,
    decimal RawReadingValue,
    Guid? OperatorId);
