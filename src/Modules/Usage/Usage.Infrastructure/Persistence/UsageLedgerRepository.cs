using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IUsageLedgerRepository"/>.</summary>
public sealed class UsageLedgerRepository : IUsageLedgerRepository
{
    private readonly UsageDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UsageLedgerRepository"/> class.</summary>
    /// <param name="dbContext">The Usage module's persistence context.</param>
    public UsageLedgerRepository(UsageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Resolves the previously-flagged ordering risk (chat, 2026-09-29):
    /// EF Core's owned-collection materialization order is not
    /// guaranteed, so <see cref="global::Usage.Domain.UsageLedger.EnsureEntriesOrderedBySequence"/>
    /// is called immediately after load to re-establish ascending
    /// <c>Sequence</c> order in memory — every one of the aggregate's
    /// cursor-based operations depends on it.
    /// </remarks>
    public async Task<global::Usage.Domain.UsageLedger?> GetByIdAsync(
        global::Usage.Domain.UsageLedgerId id, CancellationToken cancellationToken = default)
    {
        var ledger = await _dbContext.UsageLedgers.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        ledger?.EnsureEntriesOrderedBySequence();
        return ledger;
    }

    /// <inheritdoc />
    public void Add(global::Usage.Domain.UsageLedger aggregate) => _dbContext.UsageLedgers.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Usage.Domain.UsageLedger aggregate) => _dbContext.UsageLedgers.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::Usage.Domain.UsageLedger aggregate) => _dbContext.UsageLedgers.Remove(aggregate);

    /// <inheritdoc />
    /// <remarks>See the ordering remark on <see cref="GetByIdAsync"/> — applies equally here.</remarks>
    public async Task<global::Usage.Domain.UsageLedger?> GetByOwnerAsync(
        global::Usage.Domain.UsageOwnerType ownerType,
        Guid ownerId,
        MeterReadingUnit unit,
        CancellationToken cancellationToken = default)
    {
        var ledger = await _dbContext.UsageLedgers.FirstOrDefaultAsync(
            l => l.OwnerType == ownerType && l.OwnerId == ownerId && l.Unit == unit,
            cancellationToken);
        ledger?.EnsureEntriesOrderedBySequence();
        return ledger;
    }
}
