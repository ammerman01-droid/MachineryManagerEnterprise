using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.SearchFuelConsumptionsByAsset;
using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Consumption.Application.Abstractions;

/// <summary>Repository contract for the <see cref="FuelConsumption"/> aggregate.</summary>
public interface IFuelConsumptionRepository : IRepository<FuelConsumption, FuelConsumptionId>
{
    /// <summary>
    /// Retrieves the record immediately preceding <paramref name="beforeUtc"/>
    /// (by <see cref="FuelConsumption.RecordedAtUtc"/>) for the given
    /// Asset, used to enforce the monotonic meter-reading chain (chat,
    /// 2026-09-15 — checked only against immediate neighbors, not the
    /// full chain). <paramref name="excludingId"/> excludes the record
    /// being edited from its own neighbor search.
    /// </summary>
    Task<FuelConsumption?> GetPreviousAsync(
        Guid assetId,
        DateTimeOffset beforeUtc,
        Guid? excludingId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the record immediately following <paramref name="afterUtc"/>
    /// (by <see cref="FuelConsumption.RecordedAtUtc"/>) for the given
    /// Asset, used to enforce the monotonic meter-reading chain.
    /// </summary>
    Task<FuelConsumption?> GetNextAsync(
        Guid assetId,
        DateTimeOffset afterUtc,
        Guid? excludingId,
        CancellationToken cancellationToken = default);

    /// <summary>Performs a paginated search over the fuel-consumption history of a single Asset.</summary>
    Task<SearchFuelConsumptionsByAssetResponse> SearchByAssetAsync(
        Guid assetId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
