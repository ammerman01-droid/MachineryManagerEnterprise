using FluentValidation;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.CancelDayOverride;

/// <summary>Validates <see cref="CancelDayOverrideCommand"/> per ADR-0036.</summary>
public sealed class CancelDayOverrideCommandValidator : AbstractValidator<CancelDayOverrideCommand>
{
    /// <summary>Initializes validation rules for the cancel day override command.</summary>
    public CancelDayOverrideCommandValidator()
    {
        RuleFor(x => x.WorkCalendarId).NotEmpty();
        RuleFor(x => x.DayOverrideId).NotEmpty();
    }
}