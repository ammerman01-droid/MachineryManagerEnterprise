using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.UpdateFuelType;

/// <summary>Validates <see cref="UpdateFuelTypeCommand"/>.</summary>
public sealed class UpdateFuelTypeCommandValidator : AbstractValidator<UpdateFuelTypeCommand>
{
    /// <summary>Initializes a new instance of the <see cref="UpdateFuelTypeCommandValidator"/> class.</summary>
    public UpdateFuelTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.FuelType.MaxNameLength);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Kind).IsInEnum();
    }
}
