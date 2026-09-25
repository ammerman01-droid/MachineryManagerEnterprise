namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Commands;

/// <summary>Input for one independent overflow occurrence within a Lubricant Overflow Report.</summary>
public sealed record LubricantOverflowLineInput(Guid OverflowComponentId, Guid LubricantTypeId, decimal AmountInLiters, string Reason);

/// <summary>Input for one Personnel's involvement in a Lubricant Overflow Report, recorded once for the whole report.</summary>
public sealed record LubricantOverflowReportPersonnelEntryInput(Guid PersonnelId, TimeOnly StartTime, TimeSpan Duration);
