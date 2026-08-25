namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>Request body for renaming an Organization.</summary>
/// <param name="Name">The new name.</param>
public sealed record RenameOrganizationRequest(string Name);