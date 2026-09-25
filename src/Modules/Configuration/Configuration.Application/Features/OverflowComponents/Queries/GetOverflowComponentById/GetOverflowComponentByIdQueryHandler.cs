using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Queries.GetOverflowComponentById;

/// <summary>Handles <see cref="GetOverflowComponentByIdQuery"/>.</summary>
public sealed class GetOverflowComponentByIdQueryHandler : IRequestHandler<GetOverflowComponentByIdQuery, Result<OverflowComponentDto>>
{
    private const string RequiredPermission = "OverflowComponent.View";

    private readonly IOverflowComponentRepository _overflowComponentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetOverflowComponentByIdQueryHandler"/> class.</summary>
    public GetOverflowComponentByIdQueryHandler(
        IOverflowComponentRepository overflowComponentRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _overflowComponentRepository = overflowComponentRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetOverflowComponentByIdQuery"/>.</summary>
    public async Task<Result<OverflowComponentDto>> Handle(GetOverflowComponentByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<OverflowComponentDto>(global::Configuration.Domain.OverflowComponentErrors.NotAuthorized());
        }

        var entity = await _overflowComponentRepository.GetByIdAsync(
            global::Configuration.Domain.OverflowComponentId.From(request.OverflowComponentId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure<OverflowComponentDto>(Error.NotFound("OverflowComponent.NotFound", $"Overflow Component with id {request.OverflowComponentId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<OverflowComponentDto>(global::Configuration.Domain.OverflowComponentErrors.NotAuthorized());
        }

        return Result.Success(entity.Adapt<OverflowComponentDto>());
    }
}
