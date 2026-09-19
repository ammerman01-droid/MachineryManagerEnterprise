using Mapster;
using MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Dtos;

namespace MachineryManagerEnterprise.Configuration.Application.Mappings;

/// <summary>Registers Mapster mapping configuration for AssetOperationalStatus.</summary>
public sealed class AssetOperationalStatusMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::Configuration.Domain.AssetOperationalStatus, AssetOperationalStatusDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}