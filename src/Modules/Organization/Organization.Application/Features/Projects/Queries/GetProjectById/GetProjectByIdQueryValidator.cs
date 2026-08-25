using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.GetProjectById;

/// <summary>
/// Validates <see cref="GetProjectByIdQuery"/> per ADR-0036.
/// </summary>
public sealed class GetProjectByIdQueryValidator : AbstractValidator<GetProjectByIdQuery>
{
    /// <summary>
    /// Initializes validation rules for the get-by-id query.
    /// </summary>
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}