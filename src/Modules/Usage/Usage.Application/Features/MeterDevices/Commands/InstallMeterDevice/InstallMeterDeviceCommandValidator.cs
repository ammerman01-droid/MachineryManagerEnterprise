using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.InstallMeterDevice;

/// <summary>Validates <see cref="InstallMeterDeviceCommand"/> per ADR-0036.</summary>
public sealed class InstallMeterDeviceCommandValidator : AbstractValidator<InstallMeterDeviceCommand>
{
    /// <summary>Initializes validation rules for the install meter device command.</summary>
    public InstallMeterDeviceCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OwnerType).IsInEnum();
    }
}
