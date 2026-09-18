using FluentValidation;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RemoveWorkPattern;

/// <summary>Validates <see cref="RemoveWorkPatternCommand"/> per ADR-0036.</summary>
public sealed class RemoveWorkPatternCommandValidator : AbstractValidator<RemoveWorkPatternCommand>
{
    /// <summary>Initializes validation rules for the remove work pattern command.</summary>
    public RemoveWorkPatternCommandValidator()
    {
        RuleFor(x => x.WorkCalendarId).NotEmpty();
        RuleFor(x => x.WorkPatternId).NotEmpty();
    }
}