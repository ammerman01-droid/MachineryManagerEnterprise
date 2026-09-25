using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Queries.GetOverflowComponentsByHolding;

/// <summary>Handles <see cref="GetOverflowComponentsByHoldingQuery"/>.</summary>
public sealed class GetOverflowComponentsByHoldingQueryHandler : IRequestHandler<GetOverflowComponentsByHoldingQuery, Result<IReadOnlyList<OverflowComponentDto>>>
{
    private const string RequiredPermission = "OverflowComponent.View";

    private readonly IOverflowComponentRepository _overflowComponentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetOverflowComponentsByHoldingQueryHandler"/> class.</summary>
    public GetOverflowComponentsByHoldingQueryHandler(
        IOverflowComponentRepository overflowComponentRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _overflowComponentRepository = overflowComponentRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetOverflowComponentsByHoldingQuery"/>.</summary>
    public async Task<Result<IReadOnlyList<OverflowComponentDto>>> Handle(GetOverflowComponentsByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<OverflowComponentDto>>(global::Configuration.Domain.OverflowComponentErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<OverflowComponentDto>>(global::Configuration.Domain.OverflowComponentErrors.NotAuthorized());
        }

        var overflowComponents = await _overflowComponentRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(overflowComponents);
    }
}
