using Asset.Domain;
using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Asset.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IUnitOfMeasurementRepository"/>.</summary>
public sealed class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
{
    private readonly AssetDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="UnitOfMeasurementRepository"/> class.</summary>
    public UnitOfMeasurementRepository(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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
    public async Task<IReadOnlyList<UnitOfMeasurementDto>> GetByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.UnitsOfMeasurement
            .AsNoTracking()
            .Where(u => u.OrganizationId == organizationId)
            .OrderBy(u => u.Category)
            .ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);

        return entities
            .Select(u => new UnitOfMeasurementDto(u.Id.Value, u.Name, u.Category))
            .ToList();
    }
}