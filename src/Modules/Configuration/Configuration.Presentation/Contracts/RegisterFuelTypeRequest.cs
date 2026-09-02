namespace MachineryManager.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Fuel Type.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this fuel type.</param>
/// <param name="Name">The display name (e.g. "بنزین سوپر"), required, max 50 characters.</param>
/// <param name="Price">The price, as a whole number greater than zero.</param>
/// <param name="Kind">The fixed fuel-kind classification.</param>
public sealed record RegisterFuelTypeRequest(Guid HoldingId, string Name, long Price, MachineryManager.SharedKernel.FuelKind Kind);