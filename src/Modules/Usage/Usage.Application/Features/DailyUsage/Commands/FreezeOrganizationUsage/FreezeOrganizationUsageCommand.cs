using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.FreezeOrganizationUsage;

/// <summary>
/// Command to freeze registered usage up to and including a date for every Asset of an
/// Organization at once (chat, 2026-10-13), optionally limited to the Assets that have usage under
/// one Project. Frozen days can no longer be registered, corrected or deleted, and a freeze can
/// never be moved back.
/// </summary>
/// <param name="OrganizationId">The Organization whose usage to freeze.</param>
/// <param name="ProjectId">When supplied, only Ledgers with an entry under this Project are frozen.</param>
/// <param name="FrozenUpToDate">The last date (inclusive) to freeze; may not be in the future.</param>
public sealed record FreezeOrganizationUsageCommand(
    Guid OrganizationId,
    Guid? ProjectId,
    DateOnly FrozenUpToDate) : IRequest<Result<FreezeOrganizationUsageResult>>;

/// <summary>The outcome of a bulk freeze.</summary>
/// <param name="FrozenLedgerCount">How many Ledgers had their freeze date moved forward.</param>
/// <param name="AlreadyFrozenLedgerCount">How many Ledgers were left alone because they were already frozen to the same or a later date.</param>
public sealed record FreezeOrganizationUsageResult(int FrozenLedgerCount, int AlreadyFrozenLedgerCount);
