using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RegisterMeterDevice;

/// <summary>Validates <see cref="RegisterMeterDeviceCommand"/> per ADR-0036.</summary>
public sealed class RegisterMeterDeviceCommandValidator : AbstractValidator<RegisterMeterDeviceCommand>
{
    /// <summary>Initializes validation rules for the register meter device command.</summary>
    public RegisterMeterDeviceCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.DailyCapOverride).GreaterThan(0m).When(x => x.DailyCapOverride.HasValue);
        RuleFor(x => x.Unit).IsInEnum();
    }
}
