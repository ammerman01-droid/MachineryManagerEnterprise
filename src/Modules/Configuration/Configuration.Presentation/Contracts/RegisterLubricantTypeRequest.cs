namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Lubricant Type within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this Lubricant Type.</param>
/// <param name="Name">The display name (grade and commercial name together, e.g. "Engine Oil 15W40"), required, max 100 characters.</param>
public sealed record RegisterLubricantTypeRequest(Guid HoldingId, string Name);
