namespace MachineryManagerEnterprise.Administration.Application.Features.Access.Dtos;

/// <summary>What the signed-in user may do: their own level, position and effective permissions.</summary>
/// <param name="Level">The hierarchy level of the caller's active assignment: Platform, Holding, Organization or Project.</param>
/// <param name="HoldingId">The caller's Holding (null for Platform).</param>
/// <param name="OrganizationId">The caller's Organization (Organization/Project level).</param>
/// <param name="ProjectId">The caller's Project (Project level).</param>
/// <param name="HasWildcard">Whether the caller holds every permission within their own scope.</param>
/// <param name="Permissions">The explicitly granted permissions within the caller's own scope.</param>
public sealed record MyAccessDto(
    string Level,
    Guid? HoldingId,
    Guid? OrganizationId,
    Guid? ProjectId,
    bool HasWildcard,
    IReadOnlyList<string> Permissions);
