namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Color option.</summary>
public sealed record RegisterColorRequest(Guid OrganizationId, string Name);