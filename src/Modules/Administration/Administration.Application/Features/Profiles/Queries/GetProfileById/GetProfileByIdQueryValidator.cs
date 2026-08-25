using FluentValidation;

namespace MachineryManager.Administration.Application.Features.Profiles.Queries.GetProfileById;

/// <summary>
/// Validates <see cref="GetProfileByIdQuery"/> per ADR-0036.
/// </summary>
public sealed class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    /// <summary>
    /// Initializes validation rules for the get-by-id query.
    /// </summary>
    public GetProfileByIdQueryValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}