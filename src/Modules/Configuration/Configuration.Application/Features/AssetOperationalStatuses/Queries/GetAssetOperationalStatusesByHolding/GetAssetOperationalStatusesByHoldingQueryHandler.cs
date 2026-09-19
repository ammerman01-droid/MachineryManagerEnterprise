using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Queries.GetAssetOperationalStatusesByHolding;

/// <summary>Handles <see cref="GetAssetOperationalStatusesByHoldingQuery"/>.</summary>
public sealed class GetAssetOperationalStatusesByHoldingQueryHandler
    : IRequestHandler<GetAssetOperationalStatusesByHoldingQuery, Result<IReadOnlyList<AssetOperationalStatusDto>>>
{
    private readonly IAssetOperationalStatusRepository _repository;

    /// <summary>Initializes a new instance of the <see cref="GetAssetOperationalStatusesByHoldingQueryHandler"/> class.</summary>
    public GetAssetOperationalStatusesByHoldingQueryHandler(IAssetOperationalStatusRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Executes the query.</summary>
    public async Task<Result<IReadOnlyList<AssetOperationalStatusDto>>> Handle(
        GetAssetOperationalStatusesByHoldingQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetByHoldingAsync(request.HoldingId, cancellationToken);
        return Result.Success(items);
    }
}