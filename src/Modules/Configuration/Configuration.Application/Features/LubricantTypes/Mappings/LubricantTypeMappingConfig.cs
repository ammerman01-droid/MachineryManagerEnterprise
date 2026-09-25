using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;
using Mapster;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Mappings;

/// <summary>
/// Explicit Mapster mapping for <see cref="LubricantType"/>, because
/// <see cref="LubricantTypeId"/> is a Value Object with a private
/// constructor that Mapster's convention-based reflection cannot
/// flatten to <see cref="Guid"/> on its own.
/// </summary>
public sealed class LubricantTypeMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LubricantType, LubricantTypeDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}
