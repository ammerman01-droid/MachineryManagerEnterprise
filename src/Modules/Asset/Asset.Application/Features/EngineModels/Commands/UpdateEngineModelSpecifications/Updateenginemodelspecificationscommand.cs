using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.UpdateEngineModelSpecifications;

/// <summary>Command to update an existing Engine Model's technical specifications.</summary>
public sealed record UpdateEngineModelSpecificationsCommand(
    Guid EngineModelId,
    Guid CompanyId,
    int? CylinderCount = null,
    decimal? EngineDisplacementValue = null,
    Guid? EngineDisplacementUnitOfMeasurementId = null,
    decimal? EnginePowerValue = null,
    Guid? EnginePowerUnitOfMeasurementId = null,
    decimal? WeightValue = null,
    Guid? WeightUnitOfMeasurementId = null)
    : IRequest<Result>;