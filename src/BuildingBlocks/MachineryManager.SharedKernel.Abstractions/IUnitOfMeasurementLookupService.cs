using MachineryManager.SharedKernel;

namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Unit of Measurement existence and
/// Holding ownership. Needed by other modules (e.g. Asset, for
/// EngineModel's technical specification fields) to validate a
/// UnitOfMeasurementId they receive without depending on
/// Configuration.Domain/Infrastructure directly (chat, 2026-08-30) —
/// mirrors <see cref="IHoldingLookupService"/> and
/// <see cref="IOrganizationLookupService"/>.
/// </summary>
public interface IUnitOfMeasurementLookupService
{
    /// <summary>
    /// Determines whether a Unit of Measurement with the given
    /// identifier exists and belongs to the given Holding.
    /// </summary>
    /// <param name="unitOfMeasurementId">The Unit of Measurement's identifier.</param>
    /// <param name="holdingId">The Holding the unit is expected to belong to.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><see langword="true"/> if the unit exists and belongs to <paramref name="holdingId"/>; otherwise <see langword="false"/>.</returns>
    Task<bool> ExistsInHoldingAsync(Guid unitOfMeasurementId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether a Unit of Measurement exists, belongs to the
    /// given Holding, AND its Unit Category is of the expected physical
    /// quantity kind (chat, 2026-08-31 — e.g. so Engine Power can only
    /// accept a unit whose category Kind is Power).
    /// </summary>
    /// <param name="unitOfMeasurementId">The Unit of Measurement's identifier.</param>
    /// <param name="holdingId">The Holding the unit is expected to belong to.</param>
    /// <param name="expectedKind">The physical quantity kind the unit's category must match.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><see langword="true"/> if all three conditions hold; otherwise <see langword="false"/>.</returns>
    Task<bool> ExistsInHoldingWithKindAsync(
        Guid unitOfMeasurementId, Guid holdingId, PhysicalQuantityKind expectedKind, CancellationToken cancellationToken = default);
}