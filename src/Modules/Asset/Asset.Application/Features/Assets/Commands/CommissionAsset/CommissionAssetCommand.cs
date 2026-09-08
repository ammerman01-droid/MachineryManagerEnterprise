using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.CommissionAsset;

/// <summary>Command to complete commissioning of an Asset (Registered → Commissioned).</summary>
public sealed record CommissionAssetCommand(Guid AssetId) : IRequest<Result>;