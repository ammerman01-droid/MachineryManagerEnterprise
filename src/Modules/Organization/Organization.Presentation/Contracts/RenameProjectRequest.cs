namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>Request body for renaming a Project.</summary>
/// <param name="Name">The new name.</param>
public sealed record RenameProjectRequest(string Name);