namespace MachineryManager.Asset.Application.Features.EngineModels.Dtos;

/// <summary>Read-only view of an Engine Model.</summary>
public sealed record EngineModelDto(Guid Id, string Name, string Manufacturer);