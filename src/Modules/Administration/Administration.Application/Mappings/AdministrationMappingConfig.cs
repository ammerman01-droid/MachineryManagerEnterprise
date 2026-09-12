using Mapster;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Dtos;
using MachineryManagerEnterprise.Administration.Application.Features.UserProfileAssignments.Queries.GetUserProfileAssignmentsByUserId;

namespace MachineryManagerEnterprise.Administration.Application.Mappings;

/// <summary>
/// Registers Mapster mapping configuration for the Administration module
/// (migrated from manual DTO construction, per the project's standard
/// Mapster convention — pilot: WorkCalendar module).
/// </summary>
public sealed class AdministrationMappingConfig : IRegister
{
    /// <summary>Configures the type-mapping rules used by this module's Query handlers.</summary>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<global::Administration.Domain.Profile, ProfileDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Permissions, src => src.Permissions.ToList());

        config.NewConfig<global::Administration.Domain.UserProfileAssignment, UserProfileAssignmentDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProfileId, src => src.ProfileId.Value)
            .Map(dest => dest.ScopeLevel, src => src.Scope.Level.ToString())
            .Map(dest => dest.ScopeHoldingId, src => src.Scope.HoldingId)
            .Map(dest => dest.ScopeOrganizationId, src => src.Scope.OrganizationId)
            .Map(dest => dest.ScopeProjectId, src => src.Scope.ProjectId);
    }
}