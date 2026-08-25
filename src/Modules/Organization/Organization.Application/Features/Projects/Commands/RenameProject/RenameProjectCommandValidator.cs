using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Projects.Commands.RenameProject;

/// <summary>Validates <see cref="RenameProjectCommand"/> per ADR-0036.</summary>
public sealed class RenameProjectCommandValidator : AbstractValidator<RenameProjectCommand>
{
    /// <summary>Initializes validation rules for the rename project command.</summary>
    public RenameProjectCommandValidator()
    {
        int maxLength = global::Organization.Domain.Project.MaxNameLength;

        RuleFor(x => x.ProjectId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(maxLength)
            .WithMessage($"Project name shall not exceed {maxLength} characters.");
    }
}