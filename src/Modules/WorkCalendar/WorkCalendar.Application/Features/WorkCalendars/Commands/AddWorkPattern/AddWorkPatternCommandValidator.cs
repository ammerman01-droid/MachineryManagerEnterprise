using FluentValidation;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.AddWorkPattern;

/// <summary>Validates <see cref="AddWorkPatternCommand"/> per ADR-0036.</summary>
public sealed class AddWorkPatternCommandValidator : AbstractValidator<AddWorkPatternCommand>
{
    /// <summary>Initializes validation rules for the add work pattern command.</summary>
    public AddWorkPatternCommandValidator()
    {
        RuleFor(x => x.WorkCalendarId).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.WeeklySchedule).NotNull();
        RuleFor(x => x.WeeklySchedule.Count).Equal(7).When(x => x.WeeklySchedule is not null);
    }
}