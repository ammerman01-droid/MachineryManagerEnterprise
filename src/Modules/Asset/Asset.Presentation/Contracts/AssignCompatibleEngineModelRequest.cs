namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for marking an Engine Model compatible with an Asset Model.</summary>
public sealed record AssignCompatibleEngineModelRequest(Guid EngineModelId);