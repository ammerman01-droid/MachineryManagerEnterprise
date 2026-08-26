using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>Command to register a new Asset Model within a Holding.</summary>
public sealed record RegisterAssetModelCommand(Guid HoldingId, string Name, string Manufacturer)
    : IRequest<Result<Guid>>;