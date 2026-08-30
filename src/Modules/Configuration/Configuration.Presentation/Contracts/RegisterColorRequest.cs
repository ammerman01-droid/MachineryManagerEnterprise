namespace MachineryManager.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Color within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this Color.</param>
/// <param name="Name">The display name of the color (e.g. "Red"), required, max 50 characters.</param>
public sealed record RegisterColorRequest(Guid HoldingId, string Name);