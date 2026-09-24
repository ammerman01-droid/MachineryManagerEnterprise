using FluentValidation;
using MachineryManagerEnterprise.Consumption.Domain;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.EditFuelConsumption;

/// <summary>Validates <see cref="EditFuelConsumptionCommand"/> shape-level rules.</summary>
public sealed class EditFuelConsumptionCommandValidator : AbstractValidator<EditFuelConsumptionCommand>
{
    /// <summary>Initializes the validation rules.</summary>
    public EditFuelConsumptionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FuelTypeId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.MeterReading).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DeliveredByPersonnelId).NotEmpty();
        RuleFor(x => x.ReceivedByPersonnelId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(FuelConsumption.MaxNotesLength);
    }
}
