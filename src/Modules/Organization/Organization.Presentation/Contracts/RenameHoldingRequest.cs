namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>Request body for renaming a Holding.</summary>
/// <param name="Name">The new name.</param>
public sealed record RenameHoldingRequest(string Name);