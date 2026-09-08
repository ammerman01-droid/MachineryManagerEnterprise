using Configuration.Domain;
using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.Colors.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IColorRepository"/>.</summary>
public sealed class ColorRepository : IColorRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="ColorRepository"/> class.</summary>
    public ColorRepository(ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<Color?> GetByIdAsync(ColorId id, CancellationToken cancellationToken = default) =>
        _dbContext.Colors.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(Color aggregate) => _dbContext.Colors.Add(aggregate);

    /// <inheritdoc />
    public void Update(Color aggregate) => _dbContext.Colors.Update(aggregate);

    /// <inheritdoc />
    public void Remove(Color aggregate) => _dbContext.Colors.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<ColorDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Colors
            .AsNoTracking()
            .Where(c => c.HoldingId == holdingId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(c => new ColorDto(c.Id.Value, c.Name)).ToList();
    }
}