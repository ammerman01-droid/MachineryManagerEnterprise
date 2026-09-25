using FluentValidation;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.UpdateLubricantOverflowReport;

/// <summary>Validates <see cref="UpdateLubricantOverflowReportCommand"/>.</summary>
public sealed class UpdateLubricantOverflowReportCommandValidator : AbstractValidator<UpdateLubricantOverflowReportCommand>
{
    /// <summary>Initializes a new instance of the <see cref="UpdateLubricantOverflowReportCommandValidator"/> class.</summary>
    public UpdateLubricantOverflowReportCommandValidator()
    {
        RuleFor(x => x.LubricantOverflowReportId).NotEmpty();
        RuleFor(x => x.ReportDate).NotEmpty();
        RuleFor(x => x.HourMeterReading).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Lines).NotEmpty();

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.OverflowComponentId).NotEmpty();
            line.RuleFor(l => l.LubricantTypeId).NotEmpty();
            line.RuleFor(l => l.AmountInLiters).GreaterThan(0);
            line.RuleFor(l => l.Reason).NotEmpty().MaximumLength(global::Consumption.Domain.LubricantOverflowLine.MaxReasonLength);
        });

        RuleForEach(x => x.PersonnelEntries).ChildRules(entry =>
        {
            entry.RuleFor(e => e.PersonnelId).NotEmpty();
            entry.RuleFor(e => e.Duration).GreaterThan(TimeSpan.Zero).LessThanOrEqualTo(TimeSpan.FromHours(24));
        });
    }
}
