using FluentValidation;

namespace MachineryManager.Configuration.Application.Features.Colors.Commands.RegisterColor;

/// <summary>Validates <see cref="RegisterColorCommand"/>.</summary>
public sealed class RegisterColorCommandValidator : AbstractValidator<RegisterColorCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterColorCommandValidator"/> class.</summary>
    public RegisterColorCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.Color.MaxNameLength);
    }
}