using FluentValidation;
using MachineryManagerEnterprise.Consumption.Domain;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.RecordFuelConsumption;

/// <summary>Validates <see cref="RecordFuelConsumptionCommand"/> shape-level rules.</summary>
public sealed class RecordFuelConsumptionCommandValidator : AbstractValidator<RecordFuelConsumptionCommand>
{
    /// <summary>Initializes the validation rules.</summary>
    public RecordFuelConsumptionCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.FuelSlot).IsInEnum();
        RuleFor(x => x.FuelTypeId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.MeterReading).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DeliveredByPersonnelId).NotEmpty();
        RuleFor(x => x.ReceivedByPersonnelId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(FuelConsumption.MaxNotesLength);
    }
}
