using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup into the Asset module (chat,
/// 2026-09-16/20/22), used by other modules — Consumption and
/// Maintenance — that need to check an Asset's existence or read its
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

    /// <summary>Resolves the owning Organization's identifier for the given Asset, or <c>null</c> if the Asset does not exist.</summary>
    Task<Guid?> GetOrganizationIdAsync(Guid assetId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the Project the given Asset is currently assigned to, or
    /// <c>null</c> if the Asset does not exist. Since Asset.ProjectId can
    /// change over time (chat, 2026-09-16), this reflects the Asset's
    /// CURRENT assignment only — it is not what a Lubricant Overflow
    /// Report should permanently store (see BR-017 in the Consumption
    /// module notes); it exists so the Presentation layer can pre-fill
    /// or sanity-check the Project field when creating a report. Also
    /// the single Project-lookup method Maintenance's Work Order uses
    /// (chat, 2026-09-22) — there is deliberately no separate
    /// "GetProjectIdAsync", to avoid two methods answering the same
    /// question.
    /// </summary>
    Task<Guid?> GetCurrentProjectIdAsync(Guid assetId, CancellationToken cancellationToken = default);

    /// <summary>Gets the Asset's identification code, for display purposes in other modules.</summary>
    /// <param name="assetId">The Asset's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Asset's code, or <see langword="null"/> if the Asset does not exist.</returns>
    Task<string?> GetCodeAsync(Guid assetId, CancellationToken cancellationToken = default);

    /// <summary>Gets the Asset's display name, for display purposes in other modules.</summary>
    /// <param name="assetId">The Asset's identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The Asset's name, or <see langword="null"/> if the Asset does not exist.</returns>
    Task<string?> GetNameAsync(Guid assetId, CancellationToken cancellationToken = default);
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