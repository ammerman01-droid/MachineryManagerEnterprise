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
    /// <summary>
    /// Configures the type-mapping rules used by this module's Query handlers and repositories.
    /// </summary>
    /// <remarks>
    /// Destination DTOs in this module are immutable <c>record</c> types with a
    /// positional primary constructor. Mapster's record-projection path resolves
    /// constructor parameters using its own automatic member-matching *before*
    /// applying per-member <c>.Map()</c> overrides, so a per-member <c>.Map()</c>
    /// call is not reliable here: it still lets Mapster attempt an automatic
    /// conversion of a strongly-typed identifier (e.g. <c>HoldingId</c>) directly
    /// into the constructor's <see cref="Guid"/> parameter, which throws
    /// <c>Mapster.CompileException</c> ("Cannot convert immutable type").
    /// Each configuration below therefore uses <c>.MapWith(src =&gt; new Dto(...))</c>
    /// to fully control DTO construction and bypass that automatic path.
    /// </remarks>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::Organization.Domain.Holding, HoldingDto>()
            .MapWith(src => new HoldingDto(src.Id.Value, src.Name));

        // TODO (blocked — awaiting OrganizationDto.cs to confirm its exact
        // constructor parameter list before rewriting this mapping):
        config.NewConfig<global::Organization.Domain.Organization, OrganizationDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.HoldingId, src => src.HoldingId == null ? (Guid?)null : src.HoldingId.Value);

        // TODO (blocked — awaiting ProjectDto.cs to confirm its exact
        // constructor parameter list before rewriting this mapping):
        config.NewConfig<global::Organization.Domain.Project, ProjectDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrganizationId, src => src.OrganizationId.Value);
    }
}