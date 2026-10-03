namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>
/// Read-only projection over Usage Ledger entries — same "materialize
/// via EF Core with <c>AsNoTracking</c>, then map" convention as
/// <c>AssetRepository.SearchAsync</c> and its siblings, not a
/// Dapper-backed reader. Kept as its own abstraction rather than a
/// method on <see cref="IUsageLedgerRepository"/> because, unlike
/// Asset's same-aggregate searches, this one crosses aggregate
/// boundaries: a single Meter Device's readings can span more than one
/// Usage Ledger over its lifetime (BR-042: a device may be removed
/// from one owner and later installed on another, each owner having
/// its own Ledger), so results have to be joined from
/// <c>UsageEntry</c> rows back to whichever <c>UsageLedger</c> each
/// one belongs to. Backs the future Reporting module's need to list a
/// device's full history, filterable by date and Project.
/// </summary>
public interface IUsageReadService
{
    /// <summary>
    /// Lists the usage entries produced by a given Meter Device across
    /// every Usage Ledger it has ever contributed to, ordered by
    /// <c>EntryDate</c> then <c>ShiftIndex</c> descending (most recent
    /// first), optionally filtered by date range and Project (BR-054),
    /// and paged.
    /// </summary>
    /// <param name="meterDeviceId">The Meter Device whose entries to list.</param>
    /// <param name="fromDate">Inclusive lower bound on <c>EntryDate</c>, or <see langword="null"/> for no lower bound.</param>
    /// <param name="toDate">Inclusive upper bound on <c>EntryDate</c>, or <see langword="null"/> for no upper bound.</param>
    /// <param name="projectId">Restricts results to entries snapshotted (BR-054) under this Project, or <see langword="null"/> for every Project.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Maximum number of entries per page.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<PagedResult<UsageEntryListItem>> ListByMeterDeviceAsync(
        Guid meterDeviceId,
        DateOnly? fromDate,
        DateOnly? toDate,
        Guid? projectId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// One row of a Meter Device's usage history, as read directly off
/// the persistence store rather than through the <c>UsageLedger</c>
/// aggregate — see <see cref="IUsageReadService"/>.
/// </summary>
/// <param name="UsageLedgerId">The Usage Ledger this entry belongs to — a device's history may span several, across different owners over its lifetime (BR-042).</param>
/// <param name="OwnerType">The kind of owner the Usage Ledger belongs to (Asset or Tracked Component).</param>
/// <param name="OwnerId">The owner's identifier.</param>
/// <param name="EntryDate">The calendar date this entry belongs to.</param>
/// <param name="Kind">Whether this is a normal shift reading or a counter rebase.</param>
/// <param name="ShiftIndex">The Work Calendar shift index this entry reports on, or <see langword="null"/> for a rebase entry.</param>
/// <param name="ShiftStartTime">The shift's scheduled start time, or <see langword="null"/> for a rebase entry — lets the Presentation layer prefill <c>CorrectEntryDetailsDialog</c>.</param>
/// <param name="ShiftEndTime">The shift's scheduled end time, or <see langword="null"/> for a rebase entry.</param>
/// <param name="RawReadingValue">The raw value shown on the device for this entry.</param>
/// <param name="OperationalUsageAmount">The Operational Usage contributed by this entry.</param>
/// <param name="OperatorId">The operator this shift is attributed to, when supplied.</param>
/// <param name="ProjectId">The Project this entry was snapshotted under at registration time (BR-054).</param>
/// <param name="Origin">Whether this entry was entered by a user or auto-generated to fill a missed mandatory shift (BR-044).</param>
public sealed record UsageEntryListItem(
    Guid UsageLedgerId,
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    DateOnly EntryDate,
    global::Usage.Domain.UsageEntryKind Kind,
    int? ShiftIndex,
    TimeOnly? ShiftStartTime,
    TimeOnly? ShiftEndTime,
    decimal RawReadingValue,
    decimal OperationalUsageAmount,
    Guid? OperatorId,
    Guid ProjectId,
    global::Usage.Domain.UsageEntryOrigin Origin);

/// <summary>
/// A single page of a larger result set, with the total count of
/// matching rows across every page (kept local to the Usage module for
/// now; move to SharedKernel.Abstractions if a second module needs the
/// same shape).
/// </summary>
/// <typeparam name="T">The type of item in this page.</typeparam>
/// <param name="Items">The rows on this page.</param>
/// <param name="TotalCount">The total number of rows matching the query, across every page.</param>
/// <param name="Page">The 1-based page number this result represents.</param>
/// <param name="PageSize">The maximum number of rows per page that was requested.</param>
public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
