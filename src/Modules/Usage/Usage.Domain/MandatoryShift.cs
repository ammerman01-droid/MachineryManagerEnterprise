namespace Usage.Domain;

/// <summary>
/// A single mandatory Work Calendar shift, resolved by the Application
/// layer (via <c>IWorkCalendarLookupService</c>) and passed into
/// <see cref="UsageLedger.RegisterShiftReading"/> or
/// <see cref="UsageLedger.RebaseCounter"/> to describe a shift that
/// falls between the last recorded entry and the one being registered
/// (chat, 2026-09-29). This aggregate never resolves the Work Calendar
/// itself — Usage.Domain must not depend on WorkCalendar.Domain or
/// WorkCalendar.Application (Modular Monolith boundary).
/// </summary>
/// <param name="Date">The calendar date of the shift.</param>
/// <param name="ShiftIndex">The shift's stable index within its day.</param>
/// <param name="StartTime">The shift's scheduled start time.</param>
/// <param name="EndTime">The shift's scheduled end time.</param>
public sealed record MandatoryShift(DateOnly Date, int ShiftIndex, TimeOnly StartTime, TimeOnly EndTime);
