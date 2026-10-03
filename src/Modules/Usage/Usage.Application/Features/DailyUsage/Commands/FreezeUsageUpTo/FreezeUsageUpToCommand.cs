using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.FreezeUsageUpTo;

/// <summary>
/// Command to move a Usage Ledger's freeze boundary forward (BR-052).
/// Entries dated on or before <paramref name="FrozenUpToDate"/> become
/// immutable to project-level users (BR-053). A separate,
/// project-wide bulk freeze (freezing every Asset's ledger in a
/// Project at once) is an Application Service that calls this per
/// affected ledger — not modeled here.
/// </summary>
public sealed record FreezeUsageUpToCommand(
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit,
    DateOnly FrozenUpToDate) : IRequest<Result>;
