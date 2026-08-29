using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Colors.Commands.RegisterColor;

/// <summary>Validates <see cref="RegisterColorCommand"/> per ADR-0036.</summary>
public sealed class RegisterColorCommandValidator : AbstractValidator<RegisterColorCommand>
{
    /// <summary>Initializes validation rules for the register color command.</summary>
    public RegisterColorCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.Color.MaxNameLength);
    }
}