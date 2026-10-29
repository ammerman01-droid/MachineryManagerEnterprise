namespace MachineryManagerEnterprise.Administration.Presentation.Contracts;

/// <summary>Request body for creating a Profile.</summary>
/// <param name="Name">The profile's display name.</param>
/// <param name="Permissions">The permissions the profile grants.</param>
/// <param name="HoldingId">
/// The owning Holding. Omit (null) only for a Platform-owned profile, which only Platform users may create.
/// </param>
public sealed record CreateProfileRequest(
    string Name,
    IReadOnlyList<string> Permissions,
    Guid? HoldingId = null);
