using Asset.Domain;
using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.Asset.Application.Features.EngineModels.Dtos;
using MachineryManagerEnterprise.Asset.Application.Features.EngineModels.Queries.SearchEngineModels;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Asset.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IEngineModelRepository"/>.</summary>
public sealed class EngineModelRepository : IEngineModelRepository
{
    private readonly AssetDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="EngineModelRepository"/> class.</summary>
    /// <param name="dbContext">The Asset module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public EngineModelRepository(AssetDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<EngineModel?> GetByIdAsync(EngineModelId id, CancellationToken cancellationToken = default) =>
        _dbContext.EngineModels.FirstOrDefaultAsync(em => em.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(EngineModel aggregate) => _dbContext.EngineModels.Add(aggregate);

    /// <inheritdoc />
    public void Update(EngineModel aggregate) => _dbContext.EngineModels.Update(aggregate);

    /// <inheritdoc />
    public void Remove(EngineModel aggregate) => _dbContext.EngineModels.Remove(aggregate);

    /// <inheritdoc />
    public async Task<SearchEngineModelsResponse> SearchAsync(
        Guid holdingId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.EngineModels
            .AsNoTracking()
            .Where(em => em.HoldingId == holdingId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(em => em.Name.Contains(searchTerm));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderBy(em => em.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<EngineModelDto>>(entities);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchEngineModelsResponse(
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            page < totalPages,
            page > 1);
    }
}