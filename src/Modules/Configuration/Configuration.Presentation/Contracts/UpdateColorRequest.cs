namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for <c>PUT /api/v1/colors/{id}</c>.</summary>
/// <param name="Name">The Color's new display name.</param>
public sealed record UpdateColorRequest(string Name);
