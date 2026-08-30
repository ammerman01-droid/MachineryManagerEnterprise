namespace MachineryManager.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Unit Category within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this category.</param>
/// <param name="Name">The display name of the category (e.g. "Power"), required, max 50 characters.</param>
public sealed record RegisterUnitCategoryRequest(Guid HoldingId, string Name);