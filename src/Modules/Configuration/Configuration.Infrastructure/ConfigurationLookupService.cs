using Configuration.Domain;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure;

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

    /// <inheritdoc />
    public async Task<bool> DrivingLicenseTypeExistsInHoldingAsync(Guid drivingLicenseTypeId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = DrivingLicenseTypeId.From(drivingLicenseTypeId);

        return await _dbContext.DrivingLicenseTypes
            .AsNoTracking()
            .AnyAsync(d => d.Id == id && d.HoldingId == holdingId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> JobTitleExistsInHoldingAsync(Guid jobTitleId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = JobTitleId.From(jobTitleId);

        return await _dbContext.JobTitles
            .AsNoTracking()
            .AnyAsync(j => j.Id == id && j.HoldingId == holdingId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> AssetOperationalStatusExistsInHoldingAsync(
        Guid operationalStatusId,
        Guid holdingId,
        CancellationToken cancellationToken = default)
    {
        var id = AssetOperationalStatusId.From(operationalStatusId);

        return await _dbContext.AssetOperationalStatuses
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == id &&
                     x.HoldingId == holdingId,
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FuelTypeSnapshot?> GetFuelTypeAsync(Guid fuelTypeId, CancellationToken cancellationToken = default)
    {
        var id = FuelTypeId.From(fuelTypeId);

        return await _dbContext.FuelTypes
            .AsNoTracking()
            .Where(f => f.Id == id)
            .Select(f => new FuelTypeSnapshot(f.Id.Value, f.HoldingId, f.Name, f.Price, f.Kind))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> LubricantTypeExistsInHoldingAsync(Guid lubricantTypeId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = LubricantTypeId.From(lubricantTypeId);

        return await _dbContext.LubricantTypes
            .AsNoTracking()
            .AnyAsync(l => l.Id == id && l.HoldingId == holdingId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> OverflowComponentExistsInHoldingAsync(Guid overflowComponentId, Guid holdingId, CancellationToken cancellationToken = default)
    {
        var id = OverflowComponentId.From(overflowComponentId);

        return await _dbContext.OverflowComponents
            .AsNoTracking()
            .AnyAsync(o => o.Id == id && o.HoldingId == holdingId, cancellationToken);
    }
}
