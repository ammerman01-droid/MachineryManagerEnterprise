using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.EngineModels.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Queries.GetEngineModelById;

/// <summary>
/// Handles <see cref="GetEngineModelByIdQuery"/> by loading the
/// aggregate, verifying the caller is authorized for its Holding, and
/// mapping it to a DTO.
/// </summary>
public sealed class GetEngineModelByIdQueryHandler
    : IRequestHandler<GetEngineModelByIdQuery, Result<EngineModelDto>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IEngineModelRepository _engineModelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetEngineModelByIdQueryHandler"/> class.</summary>
    public GetEngineModelByIdQueryHandler(
        IEngineModelRepository engineModelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _engineModelRepository = engineModelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<EngineModelDto>> Handle(GetEngineModelByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.EngineModelId.From(request.EngineModelId);
        var engineModel = await _engineModelRepository.GetByIdAsync(id, cancellationToken);

        if (engineModel is null)
        {
            return Result.Failure<EngineModelDto>(
                Error.NotFound("EngineModel.NotFound", $"Engine model with id {request.EngineModelId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<EngineModelDto>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(engineModel.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<EngineModelDto>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var dto = new EngineModelDto(engineModel.Id.Value, engineModel.Name, engineModel.Manufacturer);

        return Result.Success(dto);
    }
}