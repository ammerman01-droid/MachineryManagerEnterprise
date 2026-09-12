using Mapster;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Dtos;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;
using MachineryManagerEnterprise.Configuration.Application.Features.UnitsOfMeasurement.Dtos;

namespace MachineryManagerEnterprise.Configuration.Application.Mappings;

/// <summary>
/// Registers Mapster mapping configuration for the Configuration module
/// (migrated from manual DTO construction, per the project's standard
/// Mapster convention — pilot: WorkCalendar module).
/// </summary>
public sealed class ConfigurationMappingConfig : IRegister
{
    /// <summary>Configures the type-mapping rules used by this module's repositories.</summary>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::Configuration.Domain.Color, ColorDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<global::Configuration.Domain.Company, CompanyDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<global::Configuration.Domain.FuelType, FuelTypeDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<global::Configuration.Domain.UnitOfMeasurement, UnitOfMeasurementDto>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}