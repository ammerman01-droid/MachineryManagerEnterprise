namespace MachineryManager.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Unit of Measurement within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this unit.</param>
/// <param name="Name">The display name of the unit (e.g. "کیلووات"), required, max 50 characters.</param>
/// <param name="CategoryId">The identifier of an existing <c>UnitCategory</c> belonging to the same Holding.</param>
public sealed record RegisterUnitOfMeasurementRequest(Guid HoldingId, string Name, Guid CategoryId);