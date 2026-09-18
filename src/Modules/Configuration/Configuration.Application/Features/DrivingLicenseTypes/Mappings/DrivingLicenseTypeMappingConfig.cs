using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Dtos;
using Mapster;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Mappings;

/// <summary>
/// Explicit Mapster mapping for <see cref="DrivingLicenseType"/>, because
/// <see cref="DrivingLicenseTypeId"/> is a Value Object with a private
/// constructor that Mapster's convention-based reflection cannot flatten
/// to <see cref="Guid"/> on its own.
/// </summary>
public sealed class DrivingLicenseTypeMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DrivingLicenseType, DrivingLicenseTypeDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}