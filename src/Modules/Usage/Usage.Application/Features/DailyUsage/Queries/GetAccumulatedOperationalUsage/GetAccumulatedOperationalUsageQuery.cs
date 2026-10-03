using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetAccumulatedOperationalUsage;

/// <summary>
/// Query to sum a Usage Ledger's Operational Usage (BR-048) across an
/// inclusive date range. This is the read model other modules are
/// meant to consume — e.g. a future Periodic Maintenance module,
/// evaluating a usage-based service interval by passing the date of
/// the last service as <see cref="SinceDate"/> (chat, 2026-09-2x).
/// </summary>
/// <param name="OwnerType">The kind of owner the Usage Ledger belongs to (Asset or Tracked Component).</param>
/// <param name="OwnerId">The owner's identifier.</param>
/// <param name="Unit">The unit the Usage Ledger tracks (Hour, Kilometer, or Mile).</param>
/// <param name="SinceDate">Inclusive lower bound on <c>EntryDate</c>.</param>
/// <param name="ThroughDate">Inclusive upper bound on <c>EntryDate</c>, or <see langword="null"/> to include every entry up to the most recent one.</param>
public sealed record GetAccumulatedOperationalUsageQuery(
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit,
    DateOnly SinceDate,
    DateOnly? ThroughDate) : IRequest<Result<decimal>>;
