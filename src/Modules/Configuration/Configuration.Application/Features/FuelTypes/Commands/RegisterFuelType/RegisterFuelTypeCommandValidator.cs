using FluentValidation;

namespace MachineryManager.Configuration.Application.Features.FuelTypes.Commands.RegisterFuelType;

/// <summary>Validates <see cref="RegisterFuelTypeCommand"/> per ADR-0036.</summary>
public sealed class RegisterFuelTypeCommandValidator : AbstractValidator<RegisterFuelTypeCommand>
{
    /// <summary>Initializes validation rules for the register fuel type command.</summary>
    public RegisterFuelTypeCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.FuelType.MaxNameLength);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Kind).IsInEnum();
    }
}