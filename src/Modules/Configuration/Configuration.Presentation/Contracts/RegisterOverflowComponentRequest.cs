namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Overflow Component within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this Overflow Component.</param>
/// <param name="Name">The display name (e.g. "Engine", "Gearbox", "Hydraulic Tank"), required, max 100 characters.</param>
public sealed record RegisterOverflowComponentRequest(Guid HoldingId, string Name);
