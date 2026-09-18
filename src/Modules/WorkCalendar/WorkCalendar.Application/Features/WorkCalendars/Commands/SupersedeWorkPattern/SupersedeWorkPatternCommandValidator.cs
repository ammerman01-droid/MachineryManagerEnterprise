using FluentValidation;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.SupersedeWorkPattern;

/// <summary>Validates <see cref="SupersedeWorkPatternCommand"/> per ADR-0036.</summary>
public sealed class SupersedeWorkPatternCommandValidator : AbstractValidator<SupersedeWorkPatternCommand>
{
    /// <summary>Initializes validation rules for the supersede work pattern command.</summary>
    public SupersedeWorkPatternCommandValidator()
    {
        RuleFor(x => x.WorkCalendarId).NotEmpty();
        RuleFor(x => x.WorkPatternId).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.WeeklySchedule).NotNull();
        RuleFor(x => x.WeeklySchedule.Count).Equal(7).When(x => x.WeeklySchedule is not null);
    }
}