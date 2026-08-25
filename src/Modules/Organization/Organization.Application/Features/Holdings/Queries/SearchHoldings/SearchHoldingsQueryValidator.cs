using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Holdings.Queries.SearchHoldings;

/// <summary>
/// Validates <see cref="SearchHoldingsQuery"/> per ADR-0036 and API conventions.
/// </summary>
public sealed class SearchHoldingsQueryValidator : AbstractValidator<SearchHoldingsQuery>
{
    /// <summary>
    /// Initializes validation rules for the search holdings query.
    /// </summary>
    public SearchHoldingsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}