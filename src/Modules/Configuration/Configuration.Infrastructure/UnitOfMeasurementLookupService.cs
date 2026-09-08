using Configuration.Domain;
using MachineryManager.Configuration.Infrastructure.Persistence;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure;

/// <inheritdoc cref="IUnitOfMeasurementLookupService" />
public sealed class UnitOfMeasurementLookupService : IUnitOfMeasurementLookupService
{
    private readonly ConfigurationDbContext _dbContext;

/// <summary>Initializes a new instance of the <see cref="UnitOfMeasurementLookupService"/> class.</summary>
    public UnitOfMeasurementLookupService(ConfigurationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

/// <inheritdoc />
    public async Task<bool> ExistsInHoldingAsync(
        Guid unitOfMeasurementId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = UnitOfMeasurementId.From(unitOfMeasurementId);
        return await _dbContext.UnitsOfMeasurement
            .AsNoTracking()
            .AnyAsync(u => u.Id == id && u.HoldingId == holdingId, cancellationToken);
    }

/// <inheritdoc />
    public async Task<bool> ExistsInHoldingWithKindAsync(
        Guid unitOfMeasurementId,
        Guid holdingId,
        PhysicalQuantityKind expectedKind,
        CancellationToken cancellationToken = default)
    {
        var id = UnitOfMeasurementId.From(unitOfMeasurementId);
        return await _dbContext.UnitsOfMeasurement
            .AsNoTracking()
            .AnyAsync(u => u.Id == id && u.HoldingId == holdingId && u.Kind == expectedKind, cancellationToken);
    }
}