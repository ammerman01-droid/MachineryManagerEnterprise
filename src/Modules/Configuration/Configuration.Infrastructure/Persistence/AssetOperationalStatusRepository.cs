using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IAssetOperationalStatusRepository"/>.</summary>
public sealed class AssetOperationalStatusRepository : IAssetOperationalStatusRepository
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="AssetOperationalStatusRepository"/> class.</summary>
    public AssetOperationalStatusRepository(ConfigurationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<AssetOperationalStatus?> GetByIdAsync(AssetOperationalStatusId id, CancellationToken cancellationToken = default) =>
        _dbContext.AssetOperationalStatuses.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(AssetOperationalStatus aggregate) => _dbContext.AssetOperationalStatuses.Add(aggregate);

    /// <inheritdoc />
    public void Update(AssetOperationalStatus aggregate) => _dbContext.AssetOperationalStatuses.Update(aggregate);

    /// <inheritdoc />
    public void Remove(AssetOperationalStatus aggregate) => _dbContext.AssetOperationalStatuses.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<AssetOperationalStatusDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.AssetOperationalStatuses
            .AsNoTracking()
            .Where(s => s.HoldingId == holdingId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<AssetOperationalStatusDto>>(entities);
    }
}