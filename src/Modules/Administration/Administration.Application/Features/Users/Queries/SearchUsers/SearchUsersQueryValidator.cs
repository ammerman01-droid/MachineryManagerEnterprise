using FluentValidation;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.SearchUsers;

/// <summary>Validates <see cref="SearchUsersQuery"/> paging.</summary>
public sealed class SearchUsersQueryValidator : AbstractValidator<SearchUsersQuery>
{
    /// <summary>Initializes a new instance of the <see cref="SearchUsersQueryValidator"/> class.</summary>
    public SearchUsersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}
