using Configuration.Domain;
using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.FuelTypes.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IFuelTypeRepository"/>.</summary>
public sealed class FuelTypeRepository : IFuelTypeRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="FuelTypeRepository"/> class.</summary>
    /// <param name="dbContext">The Configuration module's persistence context.</param>
    public FuelTypeRepository(ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<FuelType?> GetByIdAsync(FuelTypeId id, CancellationToken cancellationToken = default) =>
        _dbContext.FuelTypes.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(FuelType aggregate) => _dbContext.FuelTypes.Add(aggregate);

    /// <inheritdoc />
    public void Update(FuelType aggregate) => _dbContext.FuelTypes.Update(aggregate);

    /// <inheritdoc />
    public void Remove(FuelType aggregate) => _dbContext.FuelTypes.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<FuelTypeDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.FuelTypes
            .AsNoTracking()
            .Where(f => f.HoldingId == holdingId)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(f => new FuelTypeDto(f.Id.Value, f.Name, f.Price, f.Kind)).ToList();
    }
}