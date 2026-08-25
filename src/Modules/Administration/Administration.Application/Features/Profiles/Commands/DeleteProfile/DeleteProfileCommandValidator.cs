using FluentValidation;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.DeleteProfile;

/// <summary>
/// Validates <see cref="DeleteProfileCommand"/> per ADR-0036.
/// </summary>
public sealed class DeleteProfileCommandValidator : AbstractValidator<DeleteProfileCommand>
{
    /// <summary>Initializes validation rules for the delete-profile command.</summary>
    public DeleteProfileCommandValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
    }
}
