using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Asset.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Asset.Domain.Color"/> aggregate.</summary>
public interface IColorRepository : IRepository<global::Asset.Domain.Color, global::Asset.Domain.ColorId>
{
    /// <summary>Retrieves every Color registered for the given Organization (used to populate selection lists).</summary>
    Task<IReadOnlyList<Features.Colors.Dtos.ColorDto>> GetByOrganizationIdAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);
}