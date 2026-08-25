namespace MachineryManager.Administration.Application.Features.Profiles.Dtos;

/// <summary>Read-only view of a Profile.</summary>
/// <param name="Id">The unique identifier of the profile.</param>
/// <param name="Name">The display name of the profile.</param>
/// <param name="Permissions">The permissions bundled in this profile.</param>
/// <param name="IsActive">Whether the profile is currently active.</param>
/// <param name="CreatedAt">The UTC timestamp when the profile was created.</param>
public sealed record ProfileDto(
    Guid Id,
    string Name,
    IReadOnlyList<string> Permissions,
    bool IsActive,
    DateTimeOffset CreatedAt);