using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.DeactivateAsset;

/// <summary>Command to temporarily take an Asset out of use (Operational → Inactive).</summary>
public sealed record DeactivateAssetCommand(Guid AssetId) : IRequest<Result>;