using Configuration.Domain;
using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.UnitCategories.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IUnitCategoryRepository"/>.</summary>
public sealed class UnitCategoryRepository : IUnitCategoryRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UnitCategoryRepository"/> class.</summary>
    public UnitCategoryRepository(ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<UnitCategory?> GetByIdAsync(UnitCategoryId id, CancellationToken cancellationToken = default) =>
        _dbContext.UnitCategories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(UnitCategory aggregate) => _dbContext.UnitCategories.Add(aggregate);

    /// <inheritdoc />
    public void Update(UnitCategory aggregate) => _dbContext.UnitCategories.Update(aggregate);

    /// <inheritdoc />
    public void Remove(UnitCategory aggregate) => _dbContext.UnitCategories.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<UnitCategoryDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.UnitCategories
            .AsNoTracking()
            .Where(c => c.HoldingId == holdingId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(c => new UnitCategoryDto(c.Id.Value, c.Name)).ToList();
    }
}