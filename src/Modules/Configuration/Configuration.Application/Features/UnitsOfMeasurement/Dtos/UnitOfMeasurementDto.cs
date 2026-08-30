namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Dtos;

/// <summary>Represents the UnitOfMeasurementDto data contract.</summary>
/// <param name="Id">The value supplied for Id.</param>
/// <param name="Name">The value supplied for Name.</param>
/// <param name="CategoryId">The value supplied for CategoryId.</param>
/// <param name="CategoryName">The value supplied for CategoryName.</param>
public sealed record UnitOfMeasurementDto(Guid Id, string Name, Guid CategoryId, string CategoryName);