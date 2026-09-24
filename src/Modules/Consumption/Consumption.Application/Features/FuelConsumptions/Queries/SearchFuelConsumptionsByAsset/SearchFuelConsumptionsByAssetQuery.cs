using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.SearchFuelConsumptionsByAsset;

/// <summary>Performs a paginated search over the fuel-consumption history of a single Asset, newest first.</summary>
public sealed record SearchFuelConsumptionsByAssetQuery(
    Guid AssetId,
    int Page = 1,
    int PageSize = 20) : IRequest<Result<SearchFuelConsumptionsByAssetResponse>>;

/// <summary>Paginated result of a fuel-consumption search.</summary>
public sealed record SearchFuelConsumptionsByAssetResponse(
    IReadOnlyList<FuelConsumptionDto> Items,
    int Page,
    int PageSize,
    int TotalItems)
{
    /// <summary>Gets the total number of pages, given <see cref="PageSize"/>.</summary>
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalItems / (double)PageSize);

    /// <summary>Gets a value indicating whether a next page exists.</summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>Gets a value indicating whether a previous page exists.</summary>
    public bool HasPreviousPage => Page > 1;
}
