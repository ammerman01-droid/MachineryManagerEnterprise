using FluentValidation;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.RevokeUserProfileAssignment;

/// <summary>
/// Validates <see cref="RevokeUserProfileAssignmentCommand"/> per ADR-0036.
/// </summary>
public sealed class RevokeUserProfileAssignmentCommandValidator
    : AbstractValidator<RevokeUserProfileAssignmentCommand>
{
    /// <summary>
    /// Initializes validation rules for the revoke-assignment command.
    /// </summary>
    public RevokeUserProfileAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
    }
}