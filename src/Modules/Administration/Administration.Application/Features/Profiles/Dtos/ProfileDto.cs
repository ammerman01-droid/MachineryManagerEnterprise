namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Dtos;

/// <summary>Read model for a Profile.</summary>
/// <param name="Id">The profile identifier.</param>
/// <param name="Name">The display name.</param>
/// <param name="Permissions">The permissions the profile grants.</param>
/// <param name="IsActive">Whether the profile can currently be assigned.</param>
/// <param name="CreatedAt">The UTC creation timestamp.</param>
/// <param name="HoldingId">The owning Holding, or <see langword="null"/> for a Platform-owned profile.</param>
/// <param name="IsSystem">Whether the profile is system-managed and therefore immutable.</param>
public sealed record ProfileDto(
    Guid Id,
    string Name,
    IReadOnlyList<string> Permissions,
    bool IsActive,
    DateTimeOffset CreatedAt,
    Guid? HoldingId = null,
    bool IsSystem = false);
