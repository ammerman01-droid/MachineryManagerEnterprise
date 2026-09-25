using FluentValidation;

namespace MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Commands.SetConsumptionFreezeThreshold;

/// <summary>Validates <see cref="SetConsumptionFreezeThresholdCommand"/>.</summary>
public sealed class SetConsumptionFreezeThresholdCommandValidator : AbstractValidator<SetConsumptionFreezeThresholdCommand>
{
    /// <summary>Initializes a new instance of the <see cref="SetConsumptionFreezeThresholdCommandValidator"/> class.</summary>
    public SetConsumptionFreezeThresholdCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.ThresholdDate).NotEmpty();
    }
}
