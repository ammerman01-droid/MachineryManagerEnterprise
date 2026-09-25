using Consumption.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Consumption.Domain;

/// <summary>
/// Aggregate Root for a single Lubricant Overflow Report: a record that,
/// for one Asset on one date/hour-meter-reading, one or more independent
/// overflow occurrences (<see cref="LubricantOverflowLine"/>) took place,
/// plus the Personnel who handled it and how long it took
/// (<see cref="LubricantOverflowReportPersonnelEntry"/>, recorded once
/// per person for the whole report). <see cref="ProjectId"/> is fixed at
/// creation time and never changes afterwards, even if the Asset is
/// later reassigned to a different Project — matching how other
/// historical Usage/Maintenance records remain permanently scoped to
/// whichever Project was current when they were created (BR-017). A
/// report can no longer be edited or deleted once its <see cref="ReportDate"/>
/// falls on or before the Organization's freeze threshold, set by an
/// Organization Administrator (chat, 2026-09-16).
/// </summary>
public sealed class LubricantOverflowReport : AggregateRoot<LubricantOverflowReportId>
{
    private readonly List<LubricantOverflowLine> _lines = [];
    private readonly List<LubricantOverflowReportPersonnelEntry> _personnelEntries = [];

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the identifier of the Project this report is permanently scoped to (fixed at creation, BR-017).</summary>
    public Guid ProjectId { get; private set; }

    /// <summary>Gets the identifier of the Asset this report is about.</summary>
    public Guid AssetId { get; private set; }

    /// <summary>Gets the date of the report.</summary>
    public DateOnly ReportDate { get; private set; }

    /// <summary>Gets the Asset's hour meter reading at the time of the report.</summary>
    public decimal HourMeterReading { get; private set; }

    /// <summary>Gets the independent overflow lines that make up this report. At least one is always present.</summary>
    public IReadOnlyCollection<LubricantOverflowLine> Lines => _lines;

    /// <summary>Gets the Personnel entries (who performed the work, and for how long) recorded once for the whole report.</summary>
    public IReadOnlyCollection<LubricantOverflowReportPersonnelEntry> PersonnelEntries => _personnelEntries;

    private LubricantOverflowReport()
    {
    }

    private LubricantOverflowReport(
        LubricantOverflowReportId id,
        Guid organizationId,
        Guid projectId,
        Guid assetId,
        DateOnly reportDate,
        decimal hourMeterReading)
        : base(id)
    {
        OrganizationId = organizationId;
        ProjectId = projectId;
        AssetId = assetId;
        ReportDate = reportDate;
        HourMeterReading = hourMeterReading;
    }

    /// <summary>
    /// Creates a new Lubricant Overflow Report with its full set of lines
    /// and personnel entries. At least one line is required; personnel
    /// entries may be empty (e.g. reported before cleanup work is assigned).
    /// </summary>
    public static Result<LubricantOverflowReport> Create(
        Guid organizationId,
        Guid projectId,
        Guid assetId,
        DateOnly reportDate,
        decimal hourMeterReading,
        IReadOnlyCollection<(Guid OverflowComponentId, Guid LubricantTypeId, decimal AmountInLiters, string Reason)> lines,
        IReadOnlyCollection<(Guid PersonnelId, TimeOnly StartTime, TimeSpan Duration)> personnelEntries,
        IDateTimeProvider dateTimeProvider)
    {
        var headerValidation = ValidateHeader(assetId, projectId, reportDate, hourMeterReading, dateTimeProvider);
        if (headerValidation.IsFailure)
        {
            return Result.Failure<LubricantOverflowReport>(headerValidation.Error);
        }

        if (lines.Count == 0)
        {
            return Result.Failure<LubricantOverflowReport>(LubricantOverflowReportErrors.AtLeastOneLineRequired());
        }

        var report = new LubricantOverflowReport(LubricantOverflowReportId.New(), organizationId, projectId, assetId, reportDate, hourMeterReading);

        foreach (var line in lines)
        {
            var lineResult = LubricantOverflowLine.Create(line.OverflowComponentId, line.LubricantTypeId, line.AmountInLiters, line.Reason);
            if (lineResult.IsFailure)
            {
                return Result.Failure<LubricantOverflowReport>(lineResult.Error);
            }

            report._lines.Add(lineResult.Value);
        }

        foreach (var entry in personnelEntries)
        {
            var entryResult = LubricantOverflowReportPersonnelEntry.Create(entry.PersonnelId, entry.StartTime, entry.Duration);
            if (entryResult.IsFailure)
            {
                return Result.Failure<LubricantOverflowReport>(entryResult.Error);
            }

            report._personnelEntries.Add(entryResult.Value);
        }

        report.RaiseDomainEvent(new LubricantOverflowReportCreated(report.Id, organizationId, projectId, assetId, reportDate, dateTimeProvider.UtcNow));

        return report;
    }

    /// <summary>
    /// Replaces the report's editable details (date, hour meter reading,
    /// lines, and personnel entries). <see cref="OrganizationId"/>,
    /// <see cref="ProjectId"/>, and <see cref="AssetId"/> can never be
    /// changed after creation. Fails with <see cref="LubricantOverflowReportErrors.Frozen"/>
    /// if <paramref name="freezeThresholdDate"/> is at or after <see cref="ReportDate"/>.
    /// </summary>
    public Result Update(
        DateOnly reportDate,
        decimal hourMeterReading,
        IReadOnlyCollection<(Guid OverflowComponentId, Guid LubricantTypeId, decimal AmountInLiters, string Reason)> lines,
        IReadOnlyCollection<(Guid PersonnelId, TimeOnly StartTime, TimeSpan Duration)> personnelEntries,
        DateOnly? freezeThresholdDate,
        IDateTimeProvider dateTimeProvider)
    {
        var modifiableCheck = EnsureModifiable(freezeThresholdDate);
        if (modifiableCheck.IsFailure)
        {
            return modifiableCheck;
        }

        var headerValidation = ValidateHeader(AssetId, ProjectId, reportDate, hourMeterReading, dateTimeProvider);
        if (headerValidation.IsFailure)
        {
            return headerValidation;
        }

        if (lines.Count == 0)
        {
            return Result.Failure(LubricantOverflowReportErrors.AtLeastOneLineRequired());
        }

        var newLines = new List<LubricantOverflowLine>(lines.Count);
        foreach (var line in lines)
        {
            var lineResult = LubricantOverflowLine.Create(line.OverflowComponentId, line.LubricantTypeId, line.AmountInLiters, line.Reason);
            if (lineResult.IsFailure)
            {
                return Result.Failure(lineResult.Error);
            }

            newLines.Add(lineResult.Value);
        }

        var newPersonnelEntries = new List<LubricantOverflowReportPersonnelEntry>(personnelEntries.Count);
        foreach (var entry in personnelEntries)
        {
            var entryResult = LubricantOverflowReportPersonnelEntry.Create(entry.PersonnelId, entry.StartTime, entry.Duration);
            if (entryResult.IsFailure)
            {
                return Result.Failure(entryResult.Error);
            }

            newPersonnelEntries.Add(entryResult.Value);
        }

        ReportDate = reportDate;
        HourMeterReading = hourMeterReading;

        _lines.Clear();
        _lines.AddRange(newLines);

        _personnelEntries.Clear();
        _personnelEntries.AddRange(newPersonnelEntries);

        RaiseDomainEvent(new LubricantOverflowReportUpdated(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Determines whether this report can currently be edited or deleted, given the Organization's freeze threshold (<c>null</c> means no freeze is set).</summary>
    public bool CanBeModified(DateOnly? freezeThresholdDate) => freezeThresholdDate is null || ReportDate > freezeThresholdDate.Value;

    /// <summary>Raises the deletion domain event. Call immediately before removing this aggregate via its repository.</summary>
    public Result MarkAsDeleted(DateOnly? freezeThresholdDate, IDateTimeProvider dateTimeProvider)
    {
        var modifiableCheck = EnsureModifiable(freezeThresholdDate);
        if (modifiableCheck.IsFailure)
        {
            return modifiableCheck;
        }

        RaiseDomainEvent(new LubricantOverflowReportDeleted(Id, OrganizationId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    private Result EnsureModifiable(DateOnly? freezeThresholdDate)
    {
        return CanBeModified(freezeThresholdDate)
            ? Result.Success()
            : Result.Failure(LubricantOverflowReportErrors.Frozen());
    }

    private static Result ValidateHeader(Guid assetId, Guid projectId, DateOnly reportDate, decimal hourMeterReading, IDateTimeProvider dateTimeProvider)
    {
        if (assetId == Guid.Empty)
        {
            return Result.Failure(LubricantOverflowReportErrors.AssetRequired());
        }

        if (projectId == Guid.Empty)
        {
            return Result.Failure(LubricantOverflowReportErrors.ProjectRequired());
        }

        if (hourMeterReading < 0)
        {
            return Result.Failure(LubricantOverflowReportErrors.HourMeterReadingInvalid());
        }

        if (reportDate > DateOnly.FromDateTime(dateTimeProvider.UtcNow.UtcDateTime))
        {
            return Result.Failure(LubricantOverflowReportErrors.ReportDateInvalid());
        }

        return Result.Success();
    }
}
