using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>
/// Repository contract for the <see cref="global::Usage.Domain.UsageLedger"/> aggregate, extending
/// the generic aggregate root repository with lookups by natural key and by Organization.
/// </summary>
public interface IUsageLedgerRepository : IRepository<global::Usage.Domain.UsageLedger, global::Usage.Domain.UsageLedgerId>
{
    /// <summary>
    /// Finds the Ledger for an owner and unit — the aggregate's natural key — or
    /// <see langword="null"/> if none exists yet. The returned Ledger's entries are in
    /// ascending <c>Sequence</c> order.
    /// </summary>
    /// <param name="ownerType">The kind of owner (Asset or Component).</param>
    /// <param name="ownerId">The owner's identifier.</param>
    /// <param name="unit">The unit the Ledger tracks.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<global::Usage.Domain.UsageLedger?> GetByOwnerAsync(
        global::Usage.Domain.UsageOwnerType ownerType,
        Guid ownerId,
        MeterReadingUnit unit,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads every Ledger of an Organization for a bulk operation such as freezing (chat,
    /// 2026-10-13), optionally narrowed to Ledgers that have at least one entry snapshotted under a
    /// given Project (BR-054). The returned Ledgers are tracked, with entries in ascending
    /// <c>Sequence</c> order.
    /// </summary>
    /// <param name="organizationId">The Organization whose Ledgers to load.</param>
    /// <param name="projectId">When supplied, only Ledgers with an entry under this Project are returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<IReadOnlyList<global::Usage.Domain.UsageLedger>> ListByOrganizationAsync(
        Guid organizationId,
        Guid? projectId,
        CancellationToken cancellationToken = default);
}
