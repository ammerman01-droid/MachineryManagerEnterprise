namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Unit of Measurement.</summary>
public sealed record RegisterUnitOfMeasurementRequest(Guid OrganizationId, string Name, string Category);