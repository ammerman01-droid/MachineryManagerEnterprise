using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Commands.RegisterAssetOperationalStatus;

/// <summary>Registers a new Asset operational status option within a Holding.</summary>
public sealed record RegisterAssetOperationalStatusCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;