using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Configuration-module master data
/// (Color, Unit of Measurement, Company, FuelType) needed by other modules.
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

    /// <summary>
    /// Retrieves a single FuelType by its identifier (chat, 2026-09-22 —
    /// replaces the earlier, never-implemented <c>GetFuelPriceAsync(holdingId,
    /// fuelKind)</c>). A Holding may register several FuelType records
    /// sharing the same <see cref="FuelKind"/> (e.g. two different diesel
    /// grades/vendors with different prices) — FuelKind is therefore only
    /// a filter dimension for presenting a picker to the user; the actual
    /// price must always be read from the specific FuelType the user
    /// selects, never derived generically from (Holding, FuelKind).
    /// </summary>
    /// <param name="fuelTypeId">The FuelType identifier to look up.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The matching <see cref="FuelTypeSnapshot"/>, or <see langword="null"/> if no FuelType with that id exists.</returns>
    Task<FuelTypeSnapshot?> GetFuelTypeAsync(Guid fuelTypeId, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the given Lubricant Type exists and belongs to the given Holding. Added (chat, 2026-09-16) for the Consumption module.</summary>
    Task<bool> LubricantTypeExistsInHoldingAsync(Guid lubricantTypeId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the given Overflow Component exists and belongs to the given Holding. Added (chat, 2026-09-16) for the Consumption module.</summary>
    Task<bool> OverflowComponentExistsInHoldingAsync(Guid overflowComponentId, Guid holdingId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Read-only snapshot of a Configuration-module FuelType, for use by
/// other modules — currently Consumption — needing to validate and
/// price a specific, user-selected FuelType at fuel-recording time
/// (chat, 2026-09-22).
/// </summary>
/// <param name="Id">The FuelType's identifier.</param>
/// <param name="HoldingId">The Holding that owns this FuelType.</param>
/// <param name="Name">The FuelType's display name.</param>
/// <param name="Price">The FuelType's current price (whole-number currency unit).</param>
/// <param name="Kind">The FuelType's fixed fuel-kind classification.</param>
public sealed record FuelTypeSnapshot(Guid Id, Guid HoldingId, string Name, long Price, FuelKind Kind);