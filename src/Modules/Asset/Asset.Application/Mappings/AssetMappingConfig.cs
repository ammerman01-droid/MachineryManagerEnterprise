using Mapster;
using MachineryManagerEnterprise.Asset.Application.Features.Assets.Dtos;
using MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Dtos;
using MachineryManagerEnterprise.Asset.Application.Features.EngineModels.Dtos;

namespace MachineryManagerEnterprise.Asset.Application.Mappings;

/// <summary>
/// Registers Mapster mapping configuration for the Asset module
/// (migrated from manual DTO construction, per the project's standard
/// Mapster convention — pilot: WorkCalendar module).
/// </summary>
public sealed class AssetMappingConfig : IRegister
{
    /// <summary>Configures the type-mapping rules used by this module's Query handlers and repositories.</summary>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::Asset.Domain.Asset, AssetDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.AssetModelId, src => src.AssetModelId.Value)
            .Map(dest => dest.Status, src => src.Status.ToString());

        config.NewConfig<global::Asset.Domain.AssetModel, AssetModelDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CompatibleEngineModelIds, src => src.CompatibleEngineModelIds.Select(x => x.Value).ToList());

        config.NewConfig<global::Asset.Domain.EngineModel, EngineModelDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}