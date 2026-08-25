using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.ReactivateOrganization;

/// <summary>Validates <see cref="ReactivateOrganizationCommand"/> per ADR-0036.</summary>
public sealed class ReactivateOrganizationCommandValidator : AbstractValidator<ReactivateOrganizationCommand>
{
    /// <summary>Initializes validation rules for the reactivate-organization command.</summary>
    public ReactivateOrganizationCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
    }
}