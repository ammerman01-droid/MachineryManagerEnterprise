using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.SetMeterDeviceDailyCap;

/// <summary>Validates <see cref="SetMeterDeviceDailyCapCommand"/> per ADR-0036.</summary>
public sealed class SetMeterDeviceDailyCapCommandValidator : AbstractValidator<SetMeterDeviceDailyCapCommand>
{
    /// <summary>Initializes validation rules for the set daily cap command.</summary>
    public SetMeterDeviceDailyCapCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
        RuleFor(x => x.DailyCapOverride).GreaterThan(0m).When(x => x.DailyCapOverride.HasValue);
    }
}
