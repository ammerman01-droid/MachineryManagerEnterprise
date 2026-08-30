using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Configuration.Application.Abstractions;

/// <summary>Represents the IUnitOfMeasurementRepository type.</summary>
public interface IUnitOfMeasurementRepository
    : IRepository<global::Configuration.Domain.UnitOfMeasurement, global::Configuration.Domain.UnitOfMeasurementId>
{
    /// <summary>
    /// Retrieves every UnitOfMeasurement registered for the given Holding.
    /// </summary>
    /// <param name="holdingId">The Holding whose unit of measurement catalog should be returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The list of unit of measurements belonging to the given Holding, ordered by name.</returns>
    Task<IReadOnlyList<(Guid Id, string Name, Guid CategoryId)>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default);
}