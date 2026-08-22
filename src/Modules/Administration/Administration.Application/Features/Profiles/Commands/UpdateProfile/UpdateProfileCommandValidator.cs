using FluentValidation;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.UpdateProfile;

/// <summary>
/// Validates <see cref="UpdateProfileCommand"/> per ADR-0036.
/// </summary>
public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    /// <summary>
    /// Initializes validation rules for the update profile command.
    /// </summary>
    public UpdateProfileCommandValidator()
    {
        int maxLength = global::Administration.Domain.Profile.MaxNameLength;

        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Profile name is required.")
            .MaximumLength(maxLength)
            .WithMessage($"Profile name shall not exceed {maxLength} characters.");
        RuleForEach(x => x.Permissions)
            .NotEmpty()
            .WithMessage("Permission cannot be empty.");
    }
}