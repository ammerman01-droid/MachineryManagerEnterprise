using FluentValidation;

namespace MachineryManager.Administration.Application.Features.Profiles.Queries.SearchProfiles;

/// <summary>Validates <see cref="SearchProfilesQuery"/> per ADR-0036.</summary>
public sealed class SearchProfilesQueryValidator : AbstractValidator<SearchProfilesQuery>
{
    /// <summary>Initializes validation rules for the search profiles query.</summary>
    public SearchProfilesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}