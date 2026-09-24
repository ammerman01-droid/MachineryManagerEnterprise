using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.SearchFuelConsumptionsByAsset;

/// <summary>Handles <see cref="SearchFuelConsumptionsByAssetQuery"/>.</summary>
public sealed class SearchFuelConsumptionsByAssetQueryHandler
    : IRequestHandler<SearchFuelConsumptionsByAssetQuery, Result<SearchFuelConsumptionsByAssetResponse>>
{
    private readonly IFuelConsumptionRepository _fuelConsumptionRepository;

    /// <summary>Initializes a new instance of the <see cref="SearchFuelConsumptionsByAssetQueryHandler"/> class.</summary>
    public SearchFuelConsumptionsByAssetQueryHandler(IFuelConsumptionRepository fuelConsumptionRepository)
    {
        _fuelConsumptionRepository = fuelConsumptionRepository;
    }

    /// <inheritdoc />
    public async Task<Result<SearchFuelConsumptionsByAssetResponse>> Handle(
        SearchFuelConsumptionsByAssetQuery request, CancellationToken cancellationToken)
    {
        var response = await _fuelConsumptionRepository.SearchByAssetAsync(
            request.AssetId, request.Page, request.PageSize, cancellationToken);

        return Result.Success(response);
    }
}
