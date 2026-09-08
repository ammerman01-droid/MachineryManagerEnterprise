using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.CommissionAsset;

/// <summary>Command to complete commissioning of an Asset (Registered → Commissioned).</summary>
public sealed record CommissionAssetCommand(Guid AssetId) : IRequest<Result>;