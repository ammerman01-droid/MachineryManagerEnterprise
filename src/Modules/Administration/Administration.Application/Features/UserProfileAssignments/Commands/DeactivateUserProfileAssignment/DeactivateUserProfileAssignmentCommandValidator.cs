using FluentValidation;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeactivateUserProfileAssignment;

/// <summary>
/// Validates <see cref="DeactivateUserProfileAssignmentCommand"/> per ADR-0036.
/// </summary>
public sealed class DeactivateUserProfileAssignmentCommandValidator
    : AbstractValidator<DeactivateUserProfileAssignmentCommand>
{
    /// <summary>
    /// Initializes validation rules for the deactivate-assignment command.
    /// </summary>
    public DeactivateUserProfileAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
    }
}