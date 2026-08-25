using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;

/// <summary>
/// Validates <see cref="SearchProjectsQuery"/> per ADR-0036 and API conventions.
/// </summary>
public sealed class SearchProjectsQueryValidator : AbstractValidator<SearchProjectsQuery>
{
    /// <summary>
    /// Initializes validation rules for the search projects query.
    /// </summary>
    public SearchProjectsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}