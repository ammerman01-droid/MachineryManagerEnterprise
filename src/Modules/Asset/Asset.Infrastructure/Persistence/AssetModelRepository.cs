using Asset.Domain;
using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Dtos;
using MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Queries.SearchAssetModels;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Asset.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IAssetModelRepository"/>.</summary>
public sealed class AssetModelRepository : IAssetModelRepository
{
    private readonly AssetDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="AssetModelRepository"/> class.</summary>
    /// <param name="dbContext">The Asset module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public AssetModelRepository(AssetDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<AssetModel?> GetByIdAsync(AssetModelId id, CancellationToken cancellationToken = default) =>
        _dbContext.AssetModels.FirstOrDefaultAsync(am => am.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(AssetModel aggregate) => _dbContext.AssetModels.Add(aggregate);

    /// <inheritdoc />
    public void Update(AssetModel aggregate) => _dbContext.AssetModels.Update(aggregate);

    /// <inheritdoc />
    public void Remove(AssetModel aggregate) => _dbContext.AssetModels.Remove(aggregate);

    /// <inheritdoc />
    public async Task<SearchAssetModelsResponse> SearchAsync(
        Guid holdingId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.AssetModels
            .AsNoTracking()
            .Where(am => am.HoldingId == holdingId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(am => am.Name.Contains(searchTerm));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        // Materialize entities first, map to DTO in memory via Mapster's
        // IMapper — avoids the EF Core 10 Select-projection translation
        // issue hit earlier with Profile.Permissions (chat, 2026-08-25).
        var entities = await query
            .OrderBy(am => am.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<AssetModelDto>>(entities);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchAssetModelsResponse(
            items, page, pageSize, totalItems, totalPages, page < totalPages, page > 1);
    }
}