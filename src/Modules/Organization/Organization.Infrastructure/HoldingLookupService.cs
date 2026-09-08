using MachineryManager.Organization.Infrastructure.Persistence;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;

namespace MachineryManager.Organization.Infrastructure;

/// <inheritdoc cref="IHoldingLookupService" />
public sealed class HoldingLookupService : IHoldingLookupService
{
    private readonly OrganizationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="HoldingLookupService"/> class.</summary>
    public HoldingLookupService(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = HoldingId.From(holdingId);

        return await _dbContext.Holdings
            .AsNoTracking()
            .AnyAsync(h => h.Id == id, cancellationToken);
    }
}
