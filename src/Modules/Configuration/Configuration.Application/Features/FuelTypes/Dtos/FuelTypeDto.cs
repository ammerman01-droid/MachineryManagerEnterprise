using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;

/// <summary>Read-only projection of a FuelType for API/UI consumption.</summary>
/// <param name="Id">The Fuel Type's identifier.</param>
/// <param name="Name">The Fuel Type's display name.</param>
/// <param name="Price">The current price.</param>
/// <param name="Kind">The fixed fuel-kind classification.</param>
/// <param name="IsActive">Whether the Fuel Type is currently active (false when soft-deleted).</param>
public sealed record FuelTypeDto(Guid Id, string Name, long Price, FuelKind Kind, bool IsActive);
