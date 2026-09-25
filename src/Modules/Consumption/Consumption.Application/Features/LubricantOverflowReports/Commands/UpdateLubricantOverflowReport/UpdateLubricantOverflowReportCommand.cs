using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.UpdateLubricantOverflowReport;

/// <summary>
/// Updates an existing Lubricant Overflow Report's date, hour meter
/// reading, lines, and personnel entries. The Asset, Project, and
/// Organization can never be changed. Fails if the report's date has
/// been frozen by an Organization Administrator.
/// </summary>
public sealed record UpdateLubricantOverflowReportCommand(
    Guid LubricantOverflowReportId,
    DateOnly ReportDate,
    decimal HourMeterReading,
    IReadOnlyCollection<LubricantOverflowLineInput> Lines,
    IReadOnlyCollection<LubricantOverflowReportPersonnelEntryInput> PersonnelEntries) : IRequest<Result>;
