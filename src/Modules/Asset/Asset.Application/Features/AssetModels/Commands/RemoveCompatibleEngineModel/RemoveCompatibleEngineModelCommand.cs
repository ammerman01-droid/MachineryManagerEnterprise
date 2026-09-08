using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RemoveCompatibleEngineModel;

/// <summary>Command to remove a previously assigned Engine Model compatibility from an Asset Model.</summary>
public sealed record RemoveCompatibleEngineModelCommand(Guid AssetModelId, Guid EngineModelId) : IRequest<Result>;