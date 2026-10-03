using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RebaseCounter;

/// <summary>Validates <see cref="RebaseCounterCommand"/> per ADR-0036.</summary>
public sealed class RebaseCounterCommandValidator : AbstractValidator<RebaseCounterCommand>
{
    /// <summary>Initializes validation rules for the rebase counter command.</summary>
    public RebaseCounterCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OwnerType).IsInEnum();
        RuleFor(x => x.Unit).IsInEnum();
        RuleFor(x => x.NewMeterDeviceId).NotEmpty();
        RuleFor(x => x.NewRawReadingValue).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.TargetDate).NotEqual(default(DateOnly));
    }
}
