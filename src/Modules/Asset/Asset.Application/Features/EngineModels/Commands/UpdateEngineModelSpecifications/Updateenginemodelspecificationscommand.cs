using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.UpdateEngineModelSpecifications;

/// <summary>Command to update the technical specifications of an existing Engine Model.</summary>
public sealed record UpdateEngineModelSpecificationsCommand(
    Guid EngineModelId,
    Guid CompanyId,
    FuelKind FuelKind,
    int? CylinderCount,
    decimal? EngineDisplacementValue,
    Guid? EngineDisplacementUnitOfMeasurementId,
    decimal? EnginePowerValue,
    Guid? EnginePowerUnitOfMeasurementId,
    decimal? WeightValue,
    Guid? WeightUnitOfMeasurementId)
    : IRequest<Result>;