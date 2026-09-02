using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Configuration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Configuration.Domain.FuelType"/> aggregate.</summary>
public interface IFuelTypeRepository
    : IRepository<global::Configuration.Domain.FuelType, global::Configuration.Domain.FuelTypeId>
{
    /// <summary>Retrieves every Fuel Type registered for the given Holding.</summary>
    /// <param name="holdingId">The Holding whose fuel type catalog should be returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The list of fuel types belonging to the given Holding, ordered by name.</returns>
    Task<IReadOnlyList<Features.FuelTypes.Dtos.FuelTypeDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default);
}