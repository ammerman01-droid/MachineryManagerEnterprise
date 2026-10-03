using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryDetails;

/// <summary>Validates <see cref="CorrectEntryDetailsCommand"/> per ADR-0036.</summary>
public sealed class CorrectEntryDetailsCommandValidator : AbstractValidator<CorrectEntryDetailsCommand>
{
    /// <summary>Initializes validation rules for the correct entry details command.</summary>
    public CorrectEntryDetailsCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OwnerType).IsInEnum();
        RuleFor(x => x.Unit).IsInEnum();
        RuleFor(x => x.EntryDate).NotEqual(default(DateOnly));
        RuleFor(x => x.ShiftIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ShiftEndTime).NotEqual(x => x.ShiftStartTime);
    }
}
