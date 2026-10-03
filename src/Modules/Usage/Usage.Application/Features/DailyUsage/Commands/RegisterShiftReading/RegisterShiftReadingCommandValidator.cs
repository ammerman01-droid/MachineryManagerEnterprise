using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RegisterShiftReading;

/// <summary>Validates <see cref="RegisterShiftReadingCommand"/> per ADR-0036.</summary>
public sealed class RegisterShiftReadingCommandValidator : AbstractValidator<RegisterShiftReadingCommand>
{
    /// <summary>Initializes validation rules for the register shift reading command.</summary>
    public RegisterShiftReadingCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
        RuleFor(x => x.EntryDate).NotEqual(default(DateOnly));
        RuleFor(x => x.ShiftIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RawReadingValue).GreaterThanOrEqualTo(0m);
    }
}
