using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.SearchMeterDevices;

/// <summary>Validates <see cref="SearchMeterDevicesQuery"/> per ADR-0036.</summary>
public sealed class SearchMeterDevicesQueryValidator : AbstractValidator<SearchMeterDevicesQuery>
{
    /// <summary>Initializes validation rules for the search meter devices query.</summary>
    public SearchMeterDevicesQueryValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}
