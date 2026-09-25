namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Dtos;

/// <summary>Represents one independent overflow occurrence within a report.</summary>
public sealed record LubricantOverflowLineDto(
    Guid Id,
    Guid OverflowComponentId,
    Guid LubricantTypeId,
    decimal AmountInLiters,
    string Reason);

/// <summary>Represents one Personnel's involvement in a report, recorded once for the whole report.</summary>
public sealed record LubricantOverflowReportPersonnelEntryDto(
    Guid Id,
    Guid PersonnelId,
    TimeOnly StartTime,
    TimeSpan Duration);

/// <summary>Represents the LubricantOverflowReportDto data contract.</summary>
public sealed record LubricantOverflowReportDto(
    Guid Id,
    Guid OrganizationId,
    Guid ProjectId,
    Guid AssetId,
    DateOnly ReportDate,
    decimal HourMeterReading,
    bool CanBeModified,
    IReadOnlyCollection<LubricantOverflowLineDto> Lines,
    IReadOnlyCollection<LubricantOverflowReportPersonnelEntryDto> PersonnelEntries);
