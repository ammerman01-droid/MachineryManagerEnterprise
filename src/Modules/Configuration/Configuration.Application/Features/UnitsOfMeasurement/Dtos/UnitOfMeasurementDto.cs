using MachineryManager.SharedKernel;

namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Dtos;

/// <summary>Read-only projection of a UnitOfMeasurement for API/UI consumption.</summary>
public sealed record UnitOfMeasurementDto(Guid Id, string Name, PhysicalQuantityKind Kind);