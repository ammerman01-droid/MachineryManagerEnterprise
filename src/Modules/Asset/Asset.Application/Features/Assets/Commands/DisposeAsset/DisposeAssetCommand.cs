using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.DisposeAsset;

/// <summary>Command to mark a Retired Asset as physically disposed of (final state, BR-004).</summary>
public sealed record DisposeAssetCommand(Guid AssetId) : IRequest<Result>;