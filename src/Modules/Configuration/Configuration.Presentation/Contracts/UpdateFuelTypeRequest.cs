using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for <c>PUT /api/v1/fuel-types/{id}</c>.</summary>
/// <param name="Name">The Fuel Type's new display name.</param>
/// <param name="Price">The new price.</param>
/// <param name="Kind">The new fixed fuel-kind classification.</param>
public sealed record UpdateFuelTypeRequest(string Name, long Price, FuelKind Kind);
