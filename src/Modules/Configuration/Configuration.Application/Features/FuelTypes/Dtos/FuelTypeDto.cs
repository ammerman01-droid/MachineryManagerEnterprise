using MachineryManager.SharedKernel;

namespace MachineryManager.Configuration.Application.Features.FuelTypes.Dtos;

/// <summary>Read-only projection of a FuelType for API/UI consumption.</summary>
public sealed record FuelTypeDto(Guid Id, string Name, long Price, FuelKind Kind);