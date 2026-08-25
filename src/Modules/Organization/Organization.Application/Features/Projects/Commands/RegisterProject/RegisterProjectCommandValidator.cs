using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Projects.Commands.RegisterProject;

/// <summary>
/// Validates <see cref="RegisterProjectCommand"/> per ADR-0036.
/// </summary>
public sealed class RegisterProjectCommandValidator : AbstractValidator<RegisterProjectCommand>
{
    /// <summary>
    /// Initializes validation rules for the register project command.
    /// </summary>
    public RegisterProjectCommandValidator()
    {
        int maxLength = global::Organization.Domain.Project.MaxNameLength;

        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(maxLength)
            .WithMessage($"Project name shall not exceed {maxLength} characters.");
    }
}