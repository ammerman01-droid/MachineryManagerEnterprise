using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Configuration.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Configuration.Domain.AssetOperationalStatus"/> aggregate.</summary>
public interface IAssetOperationalStatusRepository
    : IRepository<global::Configuration.Domain.AssetOperationalStatus, global::Configuration.Domain.AssetOperationalStatusId>
{
    /// <summary>Retrieves every operational status registered for the given Holding.</summary>
    Task<IReadOnlyList<Features.AssetOperationalStatuses.Dtos.AssetOperationalStatusDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default);
}