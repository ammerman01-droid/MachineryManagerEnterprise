namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Asset Model.</summary>
public sealed record RegisterAssetModelRequest(Guid HoldingId, string Name, string Manufacturer);