using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RenameAssetModel;

/// <summary>Command to rename an existing Asset Model.</summary>
public sealed record RenameAssetModelCommand(Guid AssetModelId, string Name) : IRequest<Result>;