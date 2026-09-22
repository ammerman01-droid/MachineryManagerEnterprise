using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

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
    /// <param name="includeInactive">
    /// When <see langword="true"/>, deactivated (soft-deleted) colors are
    /// included in the result; otherwise only active colors are returned.
    /// Defaults to <see langword="false"/> so existing consumers (e.g.
    /// selection dropdowns elsewhere in the app) keep seeing only active
    /// colors without any change.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The list of colors belonging to the given Holding, ordered by name.</returns>
    Task<IReadOnlyList<Features.Colors.Dtos.ColorDto>> GetByHoldingAsync(
        Guid holdingId, bool includeInactive = false, CancellationToken cancellationToken = default);
}
