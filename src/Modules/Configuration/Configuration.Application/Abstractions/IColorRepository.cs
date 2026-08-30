using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Configuration.Application.Abstractions;

/// <summary>
/// Repository contract for the <see cref="global::Configuration.Domain.Color"/> aggregate.
/// </summary>
public interface IColorRepository
    : IRepository<global::Configuration.Domain.Color, global::Configuration.Domain.ColorId>
{
    /// <summary>
    /// Retrieves every Color registered for the given Holding.
    /// </summary>
    /// <param name="holdingId">The Holding whose color catalog should be returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The list of colors belonging to the given Holding, ordered by name.</returns>
    Task<IReadOnlyList<Features.Colors.Dtos.ColorDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default);
}
