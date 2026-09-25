namespace MachineryManagerEnterprise.Consumption.Presentation.Contracts;

/// <summary>Request body for one independent overflow occurrence within a Lubricant Overflow Report.</summary>
/// <param name="OverflowComponentId">The machine part the lubricant overflowed from.</param>
/// <param name="LubricantTypeId">The lubricant type involved.</param>
/// <param name="AmountInLiters">The overflowed amount, in liters.</param>
/// <param name="Reason">Free-text explanation for the overflow.</param>
public sealed record LubricantOverflowLineRequest(Guid OverflowComponentId, Guid LubricantTypeId, decimal AmountInLiters, string Reason);

/// <summary>Request body for one Personnel's involvement in a Lubricant Overflow Report.</summary>
/// <param name="PersonnelId">The Personnel who performed the work.</param>
/// <param name="StartTime">The clock time the work started.</param>
/// <param name="Duration">How long the work took.</param>
public sealed record LubricantOverflowReportPersonnelEntryRequest(Guid PersonnelId, TimeOnly StartTime, TimeSpan Duration);

/// <summary>Request body for creating a new Lubricant Overflow Report.</summary>
/// <param name="AssetId">The Asset the overflow occurred on.</param>
/// <param name="ProjectId">The Project this report is permanently scoped to (fixed at creation).</param>
/// <param name="ReportDate">The date of the report.</param>
/// <param name="HourMeterReading">The Asset's hour meter reading at the time of the report.</param>
/// <param name="Lines">The independent overflow lines (at least one is required).</param>
/// <param name="PersonnelEntries">The Personnel who handled it and how long it took (may be empty).</param>
public sealed record CreateLubricantOverflowReportRequest(
    Guid AssetId,
    Guid ProjectId,
    DateOnly ReportDate,
    decimal HourMeterReading,
    IReadOnlyCollection<LubricantOverflowLineRequest> Lines,
    IReadOnlyCollection<LubricantOverflowReportPersonnelEntryRequest> PersonnelEntries);

/// <summary>Request body for updating an existing Lubricant Overflow Report.</summary>
/// <param name="ReportDate">The date of the report.</param>
/// <param name="HourMeterReading">The Asset's hour meter reading at the time of the report.</param>
/// <param name="Lines">The independent overflow lines (at least one is required).</param>
/// <param name="PersonnelEntries">The Personnel who handled it and how long it took (may be empty).</param>
public sealed record UpdateLubricantOverflowReportRequest(
    DateOnly ReportDate,
    decimal HourMeterReading,
    IReadOnlyCollection<LubricantOverflowLineRequest> Lines,
    IReadOnlyCollection<LubricantOverflowReportPersonnelEntryRequest> PersonnelEntries);
