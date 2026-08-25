using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Validates <see cref="GetOrganizationByIdQuery"/> per ADR-0036.
/// </summary>
public sealed class GetOrganizationByIdQueryValidator
    : AbstractValidator<GetOrganizationByIdQuery>
{
    /// <summary>
    /// Initializes validation rules for the get-by-id query.
    /// </summary>
    public GetOrganizationByIdQueryValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty();
    }
}