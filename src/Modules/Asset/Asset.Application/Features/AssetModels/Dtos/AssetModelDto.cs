namespace MachineryManager.Asset.Application.Features.AssetModels.Dtos;

/// <summary>Read-only view of an Asset Model.</summary>
public sealed record AssetModelDto(
    Guid Id,
    string Name,
    Guid CompanyId,
    IReadOnlyCollection<Guid> CompatibleEngineModelIds);