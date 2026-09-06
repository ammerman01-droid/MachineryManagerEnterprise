namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Dtos;

/// <summary>
/// One page of audit records plus pagination metadata
/// (chat, 2026-09-05, gam 4).
/// </summary>
/// <param name="Items">The records of the requested page.</param>
/// <param name="Page">The one-based page number that was returned.</param>
/// <param name="PageSize">The page size that was applied.</param>
/// <param name="TotalCount">The total number of records matching the filter, across all pages.</param>
public sealed record AuditEntrySearchResponse(
    IReadOnlyList<AuditEntryDto> Items,
    int Page,
    int PageSize,
    int TotalCount);