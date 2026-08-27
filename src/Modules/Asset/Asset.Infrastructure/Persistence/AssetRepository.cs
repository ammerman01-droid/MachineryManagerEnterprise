using Asset.Domain;
using MachineryManager.Asset.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Asset.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IAssetRepository"/>.</summary>
public sealed class AssetRepository : IAssetRepository
{
    private readonly AssetDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="AssetRepository"/> class.</summary>
    /// <param name="dbContext">The Asset module's persistence context.</param>
    public AssetRepository(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<global::Asset.Domain.Asset?> GetByIdAsync(AssetId id, CancellationToken cancellationToken = default) =>
        _dbContext.Assets.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::Asset.Domain.Asset aggregate) => _dbContext.Assets.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::Asset.Domain.Asset aggregate) => _dbContext.Assets.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::Asset.Domain.Asset aggregate) => _dbContext.Assets.Remove(aggregate);
}