using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Queries.SearchOrganizations;

/// <summary>
/// Validates <see cref="SearchOrganizationsQuery"/> per ADR-0036 and API conventions.
/// </summary>
public sealed class SearchOrganizationsQueryValidator
    : AbstractValidator<SearchOrganizationsQuery>
{
    /// <summary>
    /// Initializes validation rules for the search organizations query.
    /// </summary>
    public SearchOrganizationsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200);
    }
}