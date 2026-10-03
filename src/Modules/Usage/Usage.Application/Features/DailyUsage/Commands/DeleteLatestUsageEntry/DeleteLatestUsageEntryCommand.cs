using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.DeleteLatestUsageEntry;

/// <summary>
/// Command to delete the most recently registered entry on a Usage
/// Ledger — a shift reading or a counter rebase (BR-049 —
/// backward-only deletion; blocked once the entry is frozen,
/// BR-052/BR-053).
/// </summary>
public sealed record DeleteLatestUsageEntryCommand(
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit,
    DateOnly EntryDate,
    int? ShiftIndex) : IRequest<Result>;
