using MachineryManager.Organization.Infrastructure.Persistence;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Organization.Domain;

namespace MachineryManager.Organization.Infrastructure;

/// <inheritdoc cref="IOrganizationLookupService" />
public sealed class OrganizationLookupService : IOrganizationLookupService
{
    private readonly OrganizationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="OrganizationLookupService"/> class.</summary>
    public OrganizationLookupService(OrganizationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<Guid?> GetHoldingIdAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var id = OrganizationId.From(organizationId);

        var holdingId = await _dbContext.Organizations
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => (Guid?)(o.HoldingId == null ? null : o.HoldingId.Value))
            .FirstOrDefaultAsync(cancellationToken);

        return holdingId;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var id = OrganizationId.From(organizationId);

        return await _dbContext.Organizations
            .AsNoTracking()
            .AnyAsync(o => o.Id == id, cancellationToken);
    }
}