namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for moving a Usage Ledger's freeze boundary forward (BR-052).</summary>
/// <param name="FrozenUpToDate">Entries dated on or before this date become immutable to project-level users (BR-053).</param>
public sealed record FreezeUsageUpToRequest(DateOnly FrozenUpToDate);
