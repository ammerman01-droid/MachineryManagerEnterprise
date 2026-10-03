using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.FreezeUsageUpTo;

/// <summary>Validates <see cref="FreezeUsageUpToCommand"/> per ADR-0036.</summary>
public sealed class FreezeUsageUpToCommandValidator : AbstractValidator<FreezeUsageUpToCommand>
{
    /// <summary>Initializes validation rules for the freeze usage up to command.</summary>
    public FreezeUsageUpToCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OwnerType).IsInEnum();
        RuleFor(x => x.Unit).IsInEnum();
        RuleFor(x => x.FrozenUpToDate).NotEqual(default(DateOnly));
    }
}
