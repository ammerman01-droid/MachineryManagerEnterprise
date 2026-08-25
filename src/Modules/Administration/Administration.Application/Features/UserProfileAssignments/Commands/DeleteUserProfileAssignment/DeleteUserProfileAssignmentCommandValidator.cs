using FluentValidation;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeleteUserProfileAssignment;

/// <summary>
/// Validates <see cref="DeleteUserProfileAssignmentCommand"/> per ADR-0036.
/// </summary>
public sealed class DeleteUserProfileAssignmentCommandValidator
    : AbstractValidator<DeleteUserProfileAssignmentCommand>
{
    /// <summary>
    /// Initializes validation rules for the delete-assignment command.
    /// </summary>
    public DeleteUserProfileAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
    }
}