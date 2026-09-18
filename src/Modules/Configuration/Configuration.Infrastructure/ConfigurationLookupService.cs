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
}