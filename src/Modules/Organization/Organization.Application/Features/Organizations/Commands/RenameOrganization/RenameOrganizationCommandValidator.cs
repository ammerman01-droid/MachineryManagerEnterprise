using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RenameOrganization;

/// <summary>Validates <see cref="RenameOrganizationCommand"/> per ADR-0036.</summary>
public sealed class RenameOrganizationCommandValidator : AbstractValidator<RenameOrganizationCommand>
{
    /// <summary>Initializes validation rules for the rename organization command.</summary>
    public RenameOrganizationCommandValidator()
    {
        int maxLength = global::Organization.Domain.Organization.MaxNameLength;

        RuleFor(x => x.OrganizationId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Organization name is required.")
            .MaximumLength(maxLength)
            .WithMessage($"Organization name shall not exceed {maxLength} characters.");
    }
}