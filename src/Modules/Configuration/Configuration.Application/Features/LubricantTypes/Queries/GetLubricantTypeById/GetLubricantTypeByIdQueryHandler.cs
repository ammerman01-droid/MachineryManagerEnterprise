using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Queries.GetLubricantTypeById;

/// <summary>Handles <see cref="GetLubricantTypeByIdQuery"/>.</summary>
public sealed class GetLubricantTypeByIdQueryHandler : IRequestHandler<GetLubricantTypeByIdQuery, Result<LubricantTypeDto>>
{
    private const string RequiredPermission = "LubricantType.View";

    private readonly ILubricantTypeRepository _lubricantTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetLubricantTypeByIdQueryHandler"/> class.</summary>
    public GetLubricantTypeByIdQueryHandler(
        ILubricantTypeRepository lubricantTypeRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _lubricantTypeRepository = lubricantTypeRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetLubricantTypeByIdQuery"/>.</summary>
    public async Task<Result<LubricantTypeDto>> Handle(GetLubricantTypeByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<LubricantTypeDto>(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        var entity = await _lubricantTypeRepository.GetByIdAsync(
            global::Configuration.Domain.LubricantTypeId.From(request.LubricantTypeId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure<LubricantTypeDto>(Error.NotFound("LubricantType.NotFound", $"Lubricant Type with id {request.LubricantTypeId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<LubricantTypeDto>(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        return Result.Success(entity.Adapt<LubricantTypeDto>());
    }
}
