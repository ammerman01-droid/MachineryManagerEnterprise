using FluentValidation;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.ActivateUserProfileAssignment;

/// <summary>
/// Validates <see cref="ActivateUserProfileAssignmentCommand"/> per ADR-0036.
/// </summary>
public sealed class ActivateUserProfileAssignmentCommandValidator
    : AbstractValidator<ActivateUserProfileAssignmentCommand>
{
    /// <summary>
    /// Initializes validation rules for the activate-assignment command.
    /// </summary>
    public ActivateUserProfileAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
    }
}