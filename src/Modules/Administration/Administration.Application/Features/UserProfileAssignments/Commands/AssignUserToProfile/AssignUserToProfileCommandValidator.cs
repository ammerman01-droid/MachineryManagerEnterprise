using FluentValidation;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.AssignUserToProfile;

/// <summary>
/// Validates <see cref="AssignUserToProfileCommand"/> per ADR-0036.
/// </summary>
public sealed class AssignUserToProfileCommandValidator : AbstractValidator<AssignUserToProfileCommand>
{
    /// <summary>
    /// Initializes validation rules for the assign-user-to-profile command.
    /// </summary>
    public AssignUserToProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.Scope).NotNull();
    }
}