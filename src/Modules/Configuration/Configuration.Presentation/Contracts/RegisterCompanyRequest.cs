namespace MachineryManager.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Company (manufacturer).</summary>
/// <param name="HoldingId">The identifier of the owning Holding.</param>
/// <param name="Name">The Company's display name.</param>
public sealed record RegisterCompanyRequest(Guid HoldingId, string Name);