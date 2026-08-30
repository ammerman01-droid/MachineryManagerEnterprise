using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Configuration.Application.Abstractions;

/// <summary>Represents the IUnitCategoryRepository type.</summary>
public interface IUnitCategoryRepository
    : IRepository<global::Configuration.Domain.UnitCategory, global::Configuration.Domain.UnitCategoryId>
{
    /// <summary>
    /// Retrieves every UnitCategory registered for the given Holding.
    /// </summary>
    /// <param name="holdingId">The Holding whose unit category catalog should be returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The list of unit categories belonging to the given Holding, ordered by name.</returns>
    Task<IReadOnlyList<Features.UnitCategories.Dtos.UnitCategoryDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default);
}