using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.RegisterAsset;

/// <summary>Command to register a new Asset within an Organization (BR-003).</summary>
public sealed record RegisterAssetCommand(
    Guid OrganizationId,
    string Code,
    Guid AssetModelId,
    string Color,
    string? SerialNumber,
    string? LicensePlate,
    int? ManufactureYear) : IRequest<Result<Guid>>;