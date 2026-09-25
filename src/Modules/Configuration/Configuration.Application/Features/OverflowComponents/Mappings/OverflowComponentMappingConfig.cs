using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;
using Mapster;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Mappings;

/// <summary>
/// Explicit Mapster mapping for <see cref="OverflowComponent"/>, because
/// <see cref="OverflowComponentId"/> is a Value Object with a private
/// constructor that Mapster's convention-based reflection cannot
/// flatten to <see cref="Guid"/> on its own.
/// </summary>
public sealed class OverflowComponentMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OverflowComponent, OverflowComponentDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}
