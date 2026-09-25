using FluentValidation;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.CreateLubricantOverflowReport;

/// <summary>Validates <see cref="CreateLubricantOverflowReportCommand"/>.</summary>
public sealed class CreateLubricantOverflowReportCommandValidator : AbstractValidator<CreateLubricantOverflowReportCommand>
{
    /// <summary>Initializes a new instance of the <see cref="CreateLubricantOverflowReportCommandValidator"/> class.</summary>
    public CreateLubricantOverflowReportCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
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
