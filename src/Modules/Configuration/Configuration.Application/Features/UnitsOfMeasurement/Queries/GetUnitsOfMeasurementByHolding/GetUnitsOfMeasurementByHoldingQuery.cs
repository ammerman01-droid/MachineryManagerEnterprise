using MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Queries.GetUnitsOfMeasurementByHolding;

/// <summary>Retrieves the list of Units of Measurement defined for a Holding, joined with their category name.</summary>
/// <param name="HoldingId">The identifier of the Holding.</param>
public sealed record GetUnitsOfMeasurementByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<UnitOfMeasurementDto>>>;