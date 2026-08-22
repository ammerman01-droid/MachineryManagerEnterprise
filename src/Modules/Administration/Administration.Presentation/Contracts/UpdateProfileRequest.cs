namespace MachineryManager.Administration.Presentation.Contracts;

/// <summary>
/// Request body for updating an existing Profile.
/// </summary>
/// <param name="Name">The new display name of the profile.</param>
/// <param name="Permissions">The replacement set of permissions.</param>
public sealed record UpdateProfileRequest(string Name, IReadOnlyList<string> Permissions);