using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.UnitsOfMeasurement.Commands.UpdateUnitOfMeasurement;

/// <summary>Updates the name and physical quantity kind of an existing Unit of Measurement (chat, 2026-09-19).</summary>
/// <param name="UnitOfMeasurementId">The identifier of the unit to update.</param>
/// <param name="Name">The new display name.</param>
/// <param name="Kind">The new physical quantity kind.</param>
public sealed record UpdateUnitOfMeasurementCommand(
    Guid UnitOfMeasurementId,
    string Name,
    PhysicalQuantityKind Kind)
    : IRequest<Result>;
