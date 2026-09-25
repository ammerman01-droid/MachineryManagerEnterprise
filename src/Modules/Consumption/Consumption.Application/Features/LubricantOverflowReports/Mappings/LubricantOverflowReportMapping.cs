using Consumption.Domain;
using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Dtos;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Mappings;

/// <summary>
/// Maps a <see cref="LubricantOverflowReport"/> to its DTO. A plain
/// extension method rather than a Mapster <c>IRegister</c> config,
/// because <see cref="LubricantOverflowReportDto.CanBeModified"/> is
/// computed against the Organization's current freeze threshold — data
/// external to the aggregate that Mapster's convention mapping has no
/// way to supply.
/// </summary>
public static class LubricantOverflowReportMapping
{
    /// <summary>Maps the report to its DTO given the Organization's current freeze threshold (or <c>null</c> if none is set).</summary>
    public static LubricantOverflowReportDto ToDto(this LubricantOverflowReport report, DateOnly? freezeThresholdDate) =>
        new(
            report.Id.Value,
            report.OrganizationId,
            report.ProjectId,
            report.AssetId,
            report.ReportDate,
            report.HourMeterReading,
            report.CanBeModified(freezeThresholdDate),
            report.Lines
                .Select(l => new LubricantOverflowLineDto(l.Id, l.OverflowComponentId, l.LubricantTypeId, l.AmountInLiters, l.Reason))
                .ToList(),
            report.PersonnelEntries
                .Select(e => new LubricantOverflowReportPersonnelEntryDto(e.Id, e.PersonnelId, e.StartTime, e.Duration))
                .ToList());
}
