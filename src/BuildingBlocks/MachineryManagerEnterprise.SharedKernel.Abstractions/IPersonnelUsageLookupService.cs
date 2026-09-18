namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup allowing other modules (Configuration)
/// to check whether one of their catalog entries is referenced by any
/// Personnel record, before allowing that entry to be deleted.
/// </summary>
public interface IPersonnelUsageLookupService
{
    /// <summary>Determines whether the given Job Title is referenced by any Personnel record.</summary>
    Task<bool> IsJobTitleInUseAsync(Guid jobTitleId, CancellationToken cancellationToken = default);

    /// <summary>Determines whether the given Driving License Type is referenced by any Personnel record.</summary>
    Task<bool> IsDrivingLicenseTypeInUseAsync(Guid drivingLicenseTypeId, CancellationToken cancellationToken = default);
}