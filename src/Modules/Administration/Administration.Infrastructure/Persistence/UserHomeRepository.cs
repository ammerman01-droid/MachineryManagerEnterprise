using MachineryManagerEnterprise.Administration.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Administration.Infrastructure.Persistence;

/// <inheritdoc cref="IUserHomeRepository" />
public sealed class UserHomeRepository : IUserHomeRepository
{
    private readonly AdministrationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UserHomeRepository"/> class.</summary>
    /// <param name="dbContext">The Administration module's persistence context.</param>
    public UserHomeRepository(AdministrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::Administration.Domain.UserHome?> GetAsync(Guid userId, CancellationToken cancellationToken = default) =>
        _dbContext.Set<global::Administration.Domain.UserHome>()
            .AsNoTracking()
            .FirstOrDefaultAsync(home => home.UserId == userId, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Administration.Domain.UserHome home) =>
        _dbContext.Set<global::Administration.Domain.UserHome>().Add(home);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Guid>> GetUserIdsAsync(
        Guid? holdingId,
        Guid? organizationId,
        CancellationToken cancellationToken = default)
    {
        var homes = _dbContext.Set<global::Administration.Domain.UserHome>().AsNoTracking();

        if (organizationId is { } organization)
        {
            homes = homes.Where(home => home.OrganizationId == organization);
        }
        else if (holdingId is { } holding)
        {
            homes = homes.Where(home => home.HoldingId == holding);
        }
        else
        {
            return [];
        }

        return await homes.Select(home => home.UserId).ToListAsync(cancellationToken);
    }
}
