using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListMeterDeviceUsageEntries;

/// <summary>
/// Query to list a Meter Device's full daily-usage history — across
/// every Usage Ledger it has ever contributed to (BR-042, see
/// <see cref="IUsageReadService"/>) — optionally filtered by date
/// range and Project (BR-054). This is the general-purpose read model
/// the future Reporting module will build on (chat, 2026-09-2x).
/// </summary>
/// <param name="MeterDeviceId">The Meter Device whose entries to list.</param>
/// <param name="FromDate">Inclusive lower bound on <c>EntryDate</c>, or <see langword="null"/> for no lower bound.</param>
/// <param name="ToDate">Inclusive upper bound on <c>EntryDate</c>, or <see langword="null"/> for no upper bound.</param>
/// <param name="ProjectId">Restricts results to entries snapshotted (BR-054) under this Project, or <see langword="null"/> for every Project.</param>
/// <param name="Page">1-based page number. Defaults to the first page.</param>
/// <param name="PageSize">Maximum number of entries per page. Defaults to 50.</param>
public sealed record ListMeterDeviceUsageEntriesQuery(
    Guid MeterDeviceId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    Guid? ProjectId,
    int Page = 1,
    int PageSize = 50) : IRequest<Result<PagedResult<UsageEntryListItem>>>;
