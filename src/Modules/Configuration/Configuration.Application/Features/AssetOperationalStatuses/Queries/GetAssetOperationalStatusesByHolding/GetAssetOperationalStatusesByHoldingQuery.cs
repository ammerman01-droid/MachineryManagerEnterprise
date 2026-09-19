using MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Queries.GetAssetOperationalStatusesByHolding;

/// <summary>Retrieves every operational status registered for a Holding.</summary>
public sealed record GetAssetOperationalStatusesByHoldingQuery(Guid HoldingId)
    : IRequest<Result<IReadOnlyList<AssetOperationalStatusDto>>>;