namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, read-only lookup for Configuration-module master data
/// (Color, Unit of Measurement) needed by other modules — e.g. Asset
/// verifying a selected Color belongs to the correct Holding — without
/// depending on Configuration.Application/Domain directly. Mirrors
/// <see cref="IOrganizationLookupService"/> and
/// <see cref="IHoldingLookupService"/> (chat, 2026-08-30).
/// </summary>
public interface IConfigurationLookupService
{
    /// <summary>Checks whether the given Color exists and belongs to the given Holding.</summary>
    Task<bool> ColorExistsInHoldingAsync(Guid colorId, Guid holdingId, CancellationToken cancellationToken = default);

    /// <summary>Checks whether the given Unit of Measurement exists and belongs to the given Holding.</summary>
    Task<bool> UnitOfMeasurementExistsInHoldingAsync(Guid unitOfMeasurementId, Guid holdingId, CancellationToken cancellationToken = default);
}