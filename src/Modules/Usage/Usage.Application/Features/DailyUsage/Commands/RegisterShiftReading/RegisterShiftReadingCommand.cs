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
public sealed record RegisterShiftReadingCommand(
    Guid MeterDeviceId,
    DateOnly EntryDate,
    int ShiftIndex,
    decimal RawReadingValue,
    Guid? OperatorId) : IRequest<Result>;
