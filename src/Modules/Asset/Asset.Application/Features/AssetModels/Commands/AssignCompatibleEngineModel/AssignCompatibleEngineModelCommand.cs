using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.AssignCompatibleEngineModel;

/// <summary>Command to mark an Engine Model compatible with an Asset Model.</summary>
public sealed record AssignCompatibleEngineModelCommand(Guid AssetModelId, Guid EngineModelId) : IRequest<Result>;