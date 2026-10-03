using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryReading;

/// <summary>
/// Command to correct an already-registered shift reading's raw value
/// (BR-051). The Ledger is addressed by its natural key (Owner + Unit)
/// rather than by a Meter Device id: the device that produced the
/// original reading may since have been removed. See
/// <see cref="global::Usage.Domain.UsageLedger.CorrectEntryReading"/>
/// for exactly how far the resulting cascade of recalculation reaches.
/// </summary>
public sealed record CorrectEntryReadingCommand(
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit,
    DateOnly EntryDate,
    int ShiftIndex,
    decimal NewRawReadingValue) : IRequest<Result>;
