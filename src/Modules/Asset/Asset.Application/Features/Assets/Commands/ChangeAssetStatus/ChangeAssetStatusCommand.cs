using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.ChangeAssetStatus;

/// <summary>Command to move an Asset to any other status (chat, 2026-09-20).</summary>
/// <param name="AssetId">The identifier of the Asset.</param>
/// <param name="NewStatus">The status to move the Asset to.</param>
public sealed record ChangeAssetStatusCommand(Guid AssetId, global::Asset.Domain.AssetStatus NewStatus)
    : IRequest<Result>;
