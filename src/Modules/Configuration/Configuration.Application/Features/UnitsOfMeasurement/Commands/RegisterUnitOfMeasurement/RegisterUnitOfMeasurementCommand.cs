using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;

/// <summary>Registers a new Unit of Measurement (e.g. "kW", "HP") within a Holding.</summary>
/// <param name="HoldingId">The identifier of the owning Holding.</param>
/// <param name="Name">The display name of the unit.</param>
/// <param name="Kind">The identifier of the Kind this unit belongs to.</param>
public sealed record RegisterUnitOfMeasurementCommand(
    Guid HoldingId,
    string Name,
    PhysicalQuantityKind Kind)
    : IRequest<Result<Guid>>;