using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;

/// <summary>Command to register a new Unit of Measurement within an Organization.</summary>
public sealed record RegisterUnitOfMeasurementCommand(Guid OrganizationId, string Name, string Category)
    : IRequest<Result<Guid>>;