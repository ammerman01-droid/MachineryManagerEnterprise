using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RegisterShiftReading;

/// <summary>
/// Command to register a reading for one Work Calendar shift on a
/// Meter Device's owner (chat, 2026-09-29 redesign to per-shift
/// granularity). Marks the device Operational and registers the
/// shift's Operational Usage on the owner's
/// <see cref="global::Usage.Domain.UsageLedger"/> in a single atomic
/// operation (BR-010 — the two aggregates are independent, but this
/// one use case touches both).
/// </summary>
/// <param name="MeterDeviceId">The device this reading is being registered for.</param>
/// <param name="EntryDate">The calendar date of the shift being reported.</param>
/// <param name="ShiftIndex">The Work Calendar shift index being reported.</param>
/// <param name="RawReadingValue">The raw value shown on the device at the end of this shift.</param>
/// <param name="OperatorId">The operator this shift is attributed to, when supplied.</param>
/// <param name="ShiftStartTime">
/// The shift's start time to record. The Work Calendar's own scheduled
/// time is used as the Presentation layer's default, but the user may
/// freely override it (chat, 2026-10-03) — this value is recorded as
/// given, not re-validated against the calendar.
/// </param>
/// <param name="ShiftEndTime">The shift's end time to record; same override behavior as <paramref name="ShiftStartTime"/>.</param>
public sealed record RegisterShiftReadingCommand(
    Guid MeterDeviceId,
    DateOnly EntryDate,
    int ShiftIndex,
    decimal RawReadingValue,
    Guid? OperatorId,
    TimeOnly ShiftStartTime,
    TimeOnly ShiftEndTime) : IRequest<Result>;
