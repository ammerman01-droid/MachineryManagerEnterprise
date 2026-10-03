using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Usage.Domain.UsageLedger"/> aggregate.</summary>
public interface IUsageLedgerRepository : IRepository<global::Usage.Domain.UsageLedger, global::Usage.Domain.UsageLedgerId>
{
    /// <summary>
    /// Retrieves the Usage Ledger for the given owner and unit — the
    /// aggregate's natural key (one Ledger per Owner+Unit) — or
    /// <see langword="null"/> if none has been created yet.
    /// </summary>
    Task<global::Usage.Domain.UsageLedger?> GetByOwnerAsync(
        global::Usage.Domain.UsageOwnerType ownerType,
        Guid ownerId,
        MeterReadingUnit unit,
        CancellationToken cancellationToken = default);
}
