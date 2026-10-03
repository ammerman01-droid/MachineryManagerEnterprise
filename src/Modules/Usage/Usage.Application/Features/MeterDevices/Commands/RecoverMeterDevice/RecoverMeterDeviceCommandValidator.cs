using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RecoverMeterDevice;

/// <summary>Validates <see cref="RecoverMeterDeviceCommand"/> per ADR-0036.</summary>
public sealed class RecoverMeterDeviceCommandValidator : AbstractValidator<RecoverMeterDeviceCommand>
{
    /// <summary>Initializes validation rules for the recover meter device command.</summary>
    public RecoverMeterDeviceCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
    }
}
