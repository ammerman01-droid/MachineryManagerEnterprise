using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <inheritdoc cref="IOverflowComponentRepository" />
public sealed class OverflowComponentRepository : IOverflowComponentRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="OverflowComponentRepository"/> class.</summary>
    public OverflowComponentRepository(ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<OverflowComponent?> GetByIdAsync(OverflowComponentId id, CancellationToken cancellationToken = default) =>
        _dbContext.OverflowComponents.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<OverflowComponentDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var overflowComponents = await _dbContext.OverflowComponents
            .AsNoTracking()
            .Where(o => o.HoldingId == holdingId)
            .OrderBy(o => o.Name)
            .Select(o => new OverflowComponentDto(o.Id.Value, o.Name))
            .ToListAsync(cancellationToken);

        return overflowComponents;
    }

    /// <inheritdoc />
    public void Add(OverflowComponent overflowComponent) => _dbContext.OverflowComponents.Add(overflowComponent);

    /// <inheritdoc />
    public void Update(OverflowComponent overflowComponent) => _dbContext.OverflowComponents.Update(overflowComponent);

    /// <inheritdoc />
    public void Remove(OverflowComponent overflowComponent) => _dbContext.OverflowComponents.Remove(overflowComponent);
}
