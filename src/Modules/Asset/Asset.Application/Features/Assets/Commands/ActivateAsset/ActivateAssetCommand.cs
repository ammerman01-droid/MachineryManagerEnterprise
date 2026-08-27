using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.ActivateAsset;

/// <summary>Command to place an Asset into operation (Commissioned or Inactive → Operational).</summary>
public sealed record ActivateAssetCommand(Guid AssetId) : IRequest<Result>;