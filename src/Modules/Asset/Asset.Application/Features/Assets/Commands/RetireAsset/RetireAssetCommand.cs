using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.RetireAsset;

/// <summary>Command to permanently withdraw an Asset from use (Operational or Inactive → Retired).</summary>
public sealed record RetireAssetCommand(Guid AssetId) : IRequest<Result>;