using FluentValidation;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Features.WorkCalendars.Commands.RegisterWorkCalendar;

/// <summary>Validates <see cref="RegisterWorkCalendarCommand"/> per ADR-0036.</summary>
public sealed class RegisterWorkCalendarCommandValidator : AbstractValidator<RegisterWorkCalendarCommand>
{
    /// <summary>Initializes validation rules for the register work calendar command.</summary>
    public RegisterWorkCalendarCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::WorkCalendar.Domain.WorkCalendar.MaxNameLength);
    }
}