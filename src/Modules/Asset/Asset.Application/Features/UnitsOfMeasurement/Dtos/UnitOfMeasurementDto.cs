namespace MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Dtos;

/// <summary>Read-only projection of a Unit of Measurement for API/UI consumption.</summary>
public sealed record UnitOfMeasurementDto(Guid Id, string Name, string Category);