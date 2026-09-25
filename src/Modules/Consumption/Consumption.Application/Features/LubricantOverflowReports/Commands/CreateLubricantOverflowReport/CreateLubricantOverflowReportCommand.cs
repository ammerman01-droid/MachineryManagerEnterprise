using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands.CreateLubricantOverflowReport;

/// <summary>
/// Creates a new Lubricant Overflow Report. <see cref="ProjectId"/> is
/// supplied explicitly by the caller (from the user's current project
/// context, or an explicit choice when the user has access to more than
/// one project) and, once created, is never changed — even if the Asset
/// is later reassigned to a different Project (BR-017).
/// </summary>
public sealed record CreateLubricantOverflowReportCommand(
    Guid AssetId,
    Guid ProjectId,
    DateOnly ReportDate,
    decimal HourMeterReading,
    IReadOnlyCollection<LubricantOverflowLineInput> Lines,
    IReadOnlyCollection<LubricantOverflowReportPersonnelEntryInput> PersonnelEntries) : IRequest<Result<Guid>>;
