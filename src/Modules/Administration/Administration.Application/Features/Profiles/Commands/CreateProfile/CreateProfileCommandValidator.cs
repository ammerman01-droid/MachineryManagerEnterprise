using FluentValidation;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.CreateProfile;

/// <summary>
/// Validates <see cref="CreateProfileCommand"/> per ADR-0036.
/// </summary>
public sealed class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    /// <summary>
    /// Initializes validation rules for the create profile command.
    /// </summary>
    public CreateProfileCommandValidator()
    {
        int maxLength = global::Administration.Domain.Profile.MaxNameLength;

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