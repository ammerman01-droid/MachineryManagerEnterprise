using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>Command to register a new Asset Model within an Organization.</summary>
public sealed record RegisterAssetModelCommand(Guid OrganizationId, string Name, string Manufacturer)
    : IRequest<Result<Guid>>;