namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>
/// Request body for registering a new Holding.
/// </summary>
/// <param name="Name">The display name of the holding to create.</param>
public sealed record RegisterHoldingRequest(string Name);