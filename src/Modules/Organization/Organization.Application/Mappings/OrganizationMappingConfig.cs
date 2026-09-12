using Mapster;
using MachineryManagerEnterprise.Organization.Application.Features.Holdings.Dtos;
using MachineryManagerEnterprise.Organization.Application.Features.Organizations.Dtos;
using MachineryManagerEnterprise.Organization.Application.Features.Projects.Dtos;

namespace MachineryManagerEnterprise.Organization.Application.Mappings;

/// <summary>
/// Registers Mapster mapping configuration for the Organization module
/// (migrated from manual DTO construction, per the project's standard
/// Mapster convention — pilot: WorkCalendar module).
/// </summary>
public sealed class OrganizationMappingConfig : IRegister
{
    /// <summary>Configures the type-mapping rules used by this module's Query handlers and repositories.</summary>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::Organization.Domain.Holding, HoldingDto>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<global::Organization.Domain.Organization, OrganizationDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.HoldingId, src => src.HoldingId == null ? (Guid?)null : src.HoldingId.Value);

        config.NewConfig<global::Organization.Domain.Project, ProjectDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrganizationId, src => src.OrganizationId.Value);
    }
}