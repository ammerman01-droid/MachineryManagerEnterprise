using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RegisterOverflowComponent;

/// <summary>Validates <see cref="RegisterOverflowComponentCommand"/>.</summary>
public sealed class RegisterOverflowComponentCommandValidator : AbstractValidator<RegisterOverflowComponentCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterOverflowComponentCommandValidator"/> class.</summary>
    public RegisterOverflowComponentCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.OverflowComponent.MaxNameLength);
    }
}
