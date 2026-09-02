using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;

/// <summary>Command to register a new Engine Model within a Holding.</summary>
public sealed record RegisterEngineModelCommand(
    Guid HoldingId,
    string Name,
    Guid CompanyId,
    int? CylinderCount = null,
    decimal? EngineDisplacementValue = null,
    Guid? EngineDisplacementUnitOfMeasurementId = null,
    decimal? EnginePowerValue = null,
    Guid? EnginePowerUnitOfMeasurementId = null,
    decimal? WeightValue = null,
    Guid? WeightUnitOfMeasurementId = null)
    : IRequest<Result<Guid>>;