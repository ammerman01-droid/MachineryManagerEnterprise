using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.SuspendOrganization;

/// <summary>Validates <see cref="SuspendOrganizationCommand"/> per ADR-0036.</summary>
public sealed class SuspendOrganizationCommandValidator : AbstractValidator<SuspendOrganizationCommand>
{
    /// <summary>Initializes validation rules for the suspend-organization command.</summary>
    public SuspendOrganizationCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
    }
}