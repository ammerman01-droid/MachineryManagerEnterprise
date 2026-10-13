using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListOrganizationUsageEntries;

/// <summary>
/// Query to list usage entries across every Meter Device belonging to
/// an Organization (chat, 2026-10-04) — backs the new org-wide "all
/// registered usage" list page.
/// </summary>
public sealed record ListOrganizationUsageEntriesQuery(
    Guid OrganizationId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    Guid? ProjectId,
    Guid? MeterDeviceId,
    int Page,
    int PageSize) : IRequest<Result<PagedResult<UsageEntryListItem>>>;
