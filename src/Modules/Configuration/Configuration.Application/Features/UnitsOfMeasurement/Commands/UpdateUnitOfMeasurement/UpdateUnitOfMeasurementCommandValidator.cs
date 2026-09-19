using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.UnitsOfMeasurement.Commands.UpdateUnitOfMeasurement;

/// <summary>Validates <see cref="UpdateUnitOfMeasurementCommand"/>.</summary>
public sealed class UpdateUnitOfMeasurementCommandValidator : AbstractValidator<UpdateUnitOfMeasurementCommand>
{
    /// <summary>Initializes a new instance of the <see cref="UpdateUnitOfMeasurementCommandValidator"/> class.</summary>
    public UpdateUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.UnitOfMeasurementId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.UnitOfMeasurement.MaxNameLength);
        RuleFor(x => x.Kind).IsInEnum();
    }
}
