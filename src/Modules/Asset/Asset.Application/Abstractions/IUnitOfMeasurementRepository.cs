using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.UnitOfMeasurement"/> aggregate.</summary>
public interface IUnitOfMeasurementRepository
    : IRepository<global::Asset.Domain.UnitOfMeasurement, global::Asset.Domain.UnitOfMeasurementId>
{
    /// <summary>Retrieves every Unit of Measurement registered for the given Organization.</summary>
    Task<IReadOnlyList<Features.UnitsOfMeasurement.Dtos.UnitOfMeasurementDto>> GetByOrganizationAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);
}