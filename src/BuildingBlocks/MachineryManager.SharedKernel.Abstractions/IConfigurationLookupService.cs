namespace MachineryManager.SharedKernel.Abstractions;

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
}