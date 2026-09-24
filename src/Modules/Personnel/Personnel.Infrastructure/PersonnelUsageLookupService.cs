using MachineryManagerEnterprise.Personnel.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Personnel.Infrastructure;

/// <inheritdoc cref="IPersonnelUsageLookupService" />
public sealed class PersonnelUsageLookupService : IPersonnelUsageLookupService
{
    private readonly PersonnelDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="PersonnelUsageLookupService"/> class.</summary>
    public PersonnelUsageLookupService(PersonnelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<bool> IsJobTitleInUseAsync(Guid jobTitleId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PersonnelRecords
            .AsNoTracking()
            .AnyAsync(p => p.JobTitleId == jobTitleId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> IsDrivingLicenseTypeInUseAsync(Guid drivingLicenseTypeId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PersonnelRecords
            .AsNoTracking()
            .AnyAsync(p => p.DrivingLicenses.Any(l => l.DrivingLicenseTypeId == drivingLicenseTypeId), cancellationToken);
    }
}
