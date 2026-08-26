namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Engine Model.</summary>
public sealed record RegisterEngineModelRequest(Guid HoldingId, string Name, string Manufacturer);