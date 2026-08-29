using FluentValidation;

namespace MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;

/// <summary>Validates <see cref="RegisterUnitOfMeasurementCommand"/> per ADR-0036.</summary>
public sealed class RegisterUnitOfMeasurementCommandValidator : AbstractValidator<RegisterUnitOfMeasurementCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUnitOfMeasurementCommandValidator"/> class.
    /// </summary>
    public RegisterUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.UnitOfMeasurement.MaxNameLength);

        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.UnitOfMeasurement.MaxCategoryLength);
    }
}