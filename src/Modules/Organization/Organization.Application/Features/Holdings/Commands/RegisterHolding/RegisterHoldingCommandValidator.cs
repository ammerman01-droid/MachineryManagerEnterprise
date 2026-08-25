using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RegisterHolding;

/// <summary>
/// Validates <see cref="RegisterHoldingCommand"/> per ADR-0036.
/// </summary>
public sealed class RegisterHoldingCommandValidator : AbstractValidator<RegisterHoldingCommand>
{
    /// <summary>
    /// Initializes validation rules for the register holding command.
    /// </summary>
    public RegisterHoldingCommandValidator()
    {
        int maxLength = global::Organization.Domain.Holding.MaxNameLength;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Holding name is required.")
            .MaximumLength(maxLength)
            .WithMessage($"Holding name shall not exceed {maxLength} characters.");
    }
}