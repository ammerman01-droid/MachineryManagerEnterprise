namespace MachineryManager.Organization.Presentation.Contracts;

/// <summary>
/// Request body for assigning an Organization to a Holding.
/// </summary>
/// <param name="HoldingId">The GUID of the target holding.</param>
public sealed record AssignOrganizationToHoldingRequest(Guid HoldingId);