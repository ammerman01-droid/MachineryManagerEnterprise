using FluentValidation;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.SearchFuelConsumptionsByAsset;

/// <summary>Validates <see cref="SearchFuelConsumptionsByAssetQuery"/> shape-level rules.</summary>
public sealed class SearchFuelConsumptionsByAssetQueryValidator : AbstractValidator<SearchFuelConsumptionsByAssetQuery>
{
    /// <summary>Initializes the validation rules.</summary>
    public SearchFuelConsumptionsByAssetQueryValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}
