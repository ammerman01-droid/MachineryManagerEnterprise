using Configuration.Domain;
using MachineryManager.Configuration.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IUnitOfMeasurementRepository"/>.</summary>
public sealed class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UnitOfMeasurementRepository"/> class.</summary>
    public UnitOfMeasurementRepository(ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<UnitOfMeasurement?> GetByIdAsync(UnitOfMeasurementId id, CancellationToken cancellationToken = default) =>
        _dbContext.UnitsOfMeasurement.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(UnitOfMeasurement aggregate) => _dbContext.UnitsOfMeasurement.Add(aggregate);

    /// <inheritdoc />
    public void Update(UnitOfMeasurement aggregate) => _dbContext.UnitsOfMeasurement.Update(aggregate);

    /// <inheritdoc />
    public void Remove(UnitOfMeasurement aggregate) => _dbContext.UnitsOfMeasurement.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<(Guid Id, string Name, Guid CategoryId)>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.UnitsOfMeasurement
            .AsNoTracking()
            .Where(u => u.HoldingId == holdingId)
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(u => (u.Id.Value, u.Name, u.CategoryId.Value)).ToList();
    }
}
