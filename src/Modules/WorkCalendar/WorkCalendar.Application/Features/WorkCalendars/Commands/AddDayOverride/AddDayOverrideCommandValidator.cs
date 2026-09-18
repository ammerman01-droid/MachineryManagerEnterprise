using FluentValidation;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.AddDayOverride;

/// <summary>Validates <see cref="AddDayOverrideCommand"/> per ADR-0036.</summary>
public sealed class AddDayOverrideCommandValidator : AbstractValidator<AddDayOverrideCommand>
{
    /// <summary>Initializes validation rules for the add day override command.</summary>
    public AddDayOverrideCommandValidator()
    {
        RuleFor(x => x.WorkCalendarId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
    }
}