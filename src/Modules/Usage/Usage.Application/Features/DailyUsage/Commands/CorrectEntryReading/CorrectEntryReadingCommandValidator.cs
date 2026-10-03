using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryReading;

/// <summary>Validates <see cref="CorrectEntryReadingCommand"/> per ADR-0036.</summary>
public sealed class CorrectEntryReadingCommandValidator : AbstractValidator<CorrectEntryReadingCommand>
{
    /// <summary>Initializes validation rules for the correct entry reading command.</summary>
    public CorrectEntryReadingCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OwnerType).IsInEnum();
        RuleFor(x => x.Unit).IsInEnum();
        RuleFor(x => x.EntryDate).NotEqual(default(DateOnly));
        RuleFor(x => x.ShiftIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.NewRawReadingValue).GreaterThanOrEqualTo(0m);
    }
}
