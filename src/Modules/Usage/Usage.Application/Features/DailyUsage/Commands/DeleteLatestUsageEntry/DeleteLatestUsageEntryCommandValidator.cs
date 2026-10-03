using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.DeleteLatestUsageEntry;

/// <summary>Validates <see cref="DeleteLatestUsageEntryCommand"/> per ADR-0036.</summary>
public sealed class DeleteLatestUsageEntryCommandValidator : AbstractValidator<DeleteLatestUsageEntryCommand>
{
    /// <summary>Initializes validation rules for the delete latest usage entry command.</summary>
    public DeleteLatestUsageEntryCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.OwnerType).IsInEnum();
        RuleFor(x => x.Unit).IsInEnum();
        RuleFor(x => x.EntryDate).NotEqual(default(DateOnly));
        RuleFor(x => x.ShiftIndex).GreaterThanOrEqualTo(0).When(x => x.ShiftIndex.HasValue);
    }
}
