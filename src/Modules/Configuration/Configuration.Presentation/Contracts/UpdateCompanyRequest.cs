namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for <c>PUT /api/v1/companies/{id}</c>.</summary>
/// <param name="Name">The Company's new display name.</param>
public sealed record UpdateCompanyRequest(string Name);
