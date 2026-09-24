using FluentValidation;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.DeleteFuelConsumption;

/// <summary>Validates <see cref="DeleteFuelConsumptionCommand"/> shape-level rules.</summary>
public sealed class DeleteFuelConsumptionCommandValidator : AbstractValidator<DeleteFuelConsumptionCommand>
{
    /// <summary>Initializes the validation rules.</summary>
    public DeleteFuelConsumptionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
