using MachineryManagerEnterprise.Personnel.Application.Features.Dtos;
using MachineryManagerEnterprise.Personnel.Domain;
using Mapster;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Mappings;

/// <summary>
/// Explicit Mapster mapping for <see cref="global::MachineryManagerEnterprise.Personnel.Domain.Personnel"/>,
/// because <see cref="PersonnelId"/> is a Value Object with a private
/// constructor that Mapster's convention-based reflection cannot flatten
/// to <see cref="Guid"/> on its own. The child <see cref="PersonnelDrivingLicense"/>
/// collection maps by convention since its own properties
/// (<c>DrivingLicenseTypeId</c>, <c>ExpiryDate</c>) are already plain
/// <see cref="Guid"/>/<see cref="DateOnly"/> — no wrapping Value Object there.
/// </summary>
public sealed class PersonnelMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::MachineryManagerEnterprise.Personnel.Domain.Personnel, PersonnelDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}