namespace MachineryManager.Administration.Presentation.Contracts;

/// <summary>
/// Request body for creating a new Profile.
/// </summary>
/// <param name="Name">The display name of the profile.</param>
/// <param name="Permissions">The initial set of permissions.</param>
public sealed record CreateProfileRequest(string Name, IReadOnlyList<string> Permissions);