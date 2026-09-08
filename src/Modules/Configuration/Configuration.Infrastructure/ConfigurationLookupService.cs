using Configuration.Domain;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure;

/// <inheritdoc cref="IConfigurationLookupService" />
public sealed class ConfigurationLookupService : IConfigurationLookupService
{
    private readonly Persistence.ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="ConfigurationLookupService"/> class.</summary>
    public ConfigurationLookupService(Persistence.ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<bool> ColorExistsInHoldingAsync(Guid colorId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = ColorId.From(colorId);
        return _dbContext.Colors.AsNoTracking().AnyAsync(c => c.Id == id && c.HoldingId == holdingId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> UnitOfMeasurementExistsInHoldingAsync(Guid unitOfMeasurementId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = UnitOfMeasurementId.From(unitOfMeasurementId);
        return _dbContext.UnitsOfMeasurement.AsNoTracking().AnyAsync(u => u.Id == id && u.HoldingId == holdingId, cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> CompanyExistsInHoldingAsync(
        Guid companyId,
        Guid holdingId,
        CancellationToken cancellationToken = default)
    {
        var id = CompanyId.From(companyId);

        return _dbContext.Companies
            .AsNoTracking()
            .AnyAsync(
                c => c.Id == id &&
                     c.HoldingId == holdingId,
                cancellationToken);
    }
}