using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Queries.GetLubricantTypesByHolding;

/// <summary>Handles <see cref="GetLubricantTypesByHoldingQuery"/>.</summary>
public sealed class GetLubricantTypesByHoldingQueryHandler : IRequestHandler<GetLubricantTypesByHoldingQuery, Result<IReadOnlyList<LubricantTypeDto>>>
{
    private const string RequiredPermission = "LubricantType.View";

    private readonly ILubricantTypeRepository _lubricantTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetLubricantTypesByHoldingQueryHandler"/> class.</summary>
    public GetLubricantTypesByHoldingQueryHandler(
        ILubricantTypeRepository lubricantTypeRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _lubricantTypeRepository = lubricantTypeRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetLubricantTypesByHoldingQuery"/>.</summary>
    public async Task<Result<IReadOnlyList<LubricantTypeDto>>> Handle(GetLubricantTypesByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<LubricantTypeDto>>(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<LubricantTypeDto>>(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        var lubricantTypes = await _lubricantTypeRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(lubricantTypes);
    }
}
