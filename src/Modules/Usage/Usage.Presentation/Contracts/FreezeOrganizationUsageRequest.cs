namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for freezing an Organization's registered usage up to a date (chat, 2026-10-13).</summary>
/// <param name="FrozenUpToDate">The last date (inclusive) to freeze; may not be in the future.</param>
/// <param name="ProjectId">When supplied, only Assets with usage under this Project are frozen.</param>
public sealed record FreezeOrganizationUsageRequest(DateOnly FrozenUpToDate, Guid? ProjectId);
