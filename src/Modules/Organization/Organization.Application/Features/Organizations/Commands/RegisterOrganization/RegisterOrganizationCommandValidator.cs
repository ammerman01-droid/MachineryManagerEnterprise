using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RegisterOrganization;

/// <summary>
/// Validates <see cref="RegisterOrganizationCommand"/> per ADR-0036.
/// </summary>
public sealed class RegisterOrganizationCommandValidator
    : AbstractValidator<RegisterOrganizationCommand>
{
    /// <summary>
    /// Initializes validation rules for the register organization command.
    /// </summary>
    public RegisterOrganizationCommandValidator()
    {
        int maxLength = global::Organization.Domain.Organization.MaxNameLength;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Organization name is required.")
            .MaximumLength(maxLength)
            .WithMessage(
                $"Organization name shall not exceed {maxLength} characters.");
    }
}