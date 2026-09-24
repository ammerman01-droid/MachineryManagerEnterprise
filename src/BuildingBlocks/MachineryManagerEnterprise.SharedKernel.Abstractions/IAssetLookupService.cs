using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup into the Asset module (chat,
/// 2026-09-16/20), used by other modules — currently Consumption — that
/// need to check an Asset's existence or read its consumption-relevant
/// fields without depending on Asset.Domain/Asset.Application directly
/// (Modular Monolith boundary — same pattern as
/// <see cref="IOrganizationLookupService"/>/<see cref="IProjectLookupService"/>).
/// </summary>
public interface IAssetLookupService
{
    /// <summary>Determines whether an Asset with the given identifier exists.</summary>
    Task<bool> ExistsAsync(Guid assetId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the subset of an Asset's fields needed to record fuel
    /// consumption against it, or <see langword="null"/> if the Asset
    /// does not exist.
    /// </summary>
    Task<AssetConsumptionSnapshot?> GetConsumptionSnapshotAsync(Guid assetId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Read-only snapshot of an Asset's consumption-relevant fields (chat,
/// 2026-09-16, updated 2026-09-20 to match Asset's fixed-enum fields —
/// MeterReadingUnit/FuelUnit replaced the earlier UnitOfMeasurement
/// Guid references).
/// </summary>
/// <param name="OrganizationId">The Asset's owning Organization.</param>
/// <param name="ProjectId">The Project the Asset is currently assigned to (always set — Asset requires a Project).</param>
/// <param name="MeterReadingUnit">The unit the Asset's meter/odometer is read in, if configured.</param>
/// <param name="PrimaryFuelKind">The Asset's primary fuel kind, if configured.</param>
/// <param name="PrimaryFuelUnit">The unit the primary fuel's quantity is counted in, if configured.</param>
/// <param name="SecondaryFuelKind">The Asset's secondary fuel kind, if this is a bi-fuel Asset.</param>
/// <param name="SecondaryFuelUnit">The unit the secondary fuel's quantity is counted in, if configured.</param>
public sealed record AssetConsumptionSnapshot(
    Guid OrganizationId,
    Guid ProjectId,
    MeterReadingUnit? MeterReadingUnit,
    FuelKind? PrimaryFuelKind,
    FuelUnit? PrimaryFuelUnit,
    FuelKind? SecondaryFuelKind,
    FuelUnit? SecondaryFuelUnit);
