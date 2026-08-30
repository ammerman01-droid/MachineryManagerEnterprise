using FluentValidation;

namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;

/// <summary>Validates <see cref="RegisterUnitOfMeasurementCommand"/>.</summary>
public sealed class RegisterUnitOfMeasurementCommandValidator : AbstractValidator<RegisterUnitOfMeasurementCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterUnitOfMeasurementCommandValidator"/> class.</summary>
    public RegisterUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.UnitOfMeasurement.MaxNameLength);
    }
}