namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Configuration-module master data
/// (Color, Unit of Measurement, Company) needed by other modules.
/// </summary>
public interface IConfigurationLookupService
{
    /// <summary>Checks whether the given Color exists and belongs to the given Holding.</summary>
    Task<bool> ColorExistsInHoldingAsync(Guid colorId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the given Unit of Measurement exists and belongs to the given Holding.</summary>
    Task<bool> UnitOfMeasurementExistsInHoldingAsync(Guid unitOfMeasurementId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the given Company exists and belongs to the given Holding.</summary>
    Task<bool> CompanyExistsInHoldingAsync(Guid companyId, Guid holdingId, CancellationToken cancellationToken = default);

     /// <summary>Checks whether the given Driving License Type exists and belongs to the given Holding.</summary>
    Task<bool> DrivingLicenseTypeExistsInHoldingAsync(Guid drivingLicenseTypeId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the given Job Title exists and belongs to the given Holding.</summary>
    Task<bool> JobTitleExistsInHoldingAsync(Guid jobTitleId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>
/// Checks whether the given Asset Operational Status exists and
/// belongs to the specified Holding (chat, 2026-09-18).
/// </summary>
/// <param name="operationalStatusId">The Asset Operational Status identifier to check.</param>
/// <param name="holdingId">The Holding the Asset Operational Status must belong to.</param>
/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
/// <returns><see langword="true"/> if the Asset Operational Status exists within the given Holding; otherwise <see langword="false"/>.</returns>
Task<bool> AssetOperationalStatusExistsInHoldingAsync(
    Guid operationalStatusId, Guid holdingId, CancellationToken cancellationToken = default);
}