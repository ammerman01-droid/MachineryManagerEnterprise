using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.Asset.Application.Features.EngineModels.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.EngineModels.Queries.GetEngineModelById;

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
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetEngineModelByIdQueryHandler"/> class.</summary>
    /// <param name="engineModelRepository">The Engine Model repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetEngineModelByIdQueryHandler(
        IEngineModelRepository engineModelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _engineModelRepository = engineModelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query containing the Engine Model identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="EngineModelDto"/> or a not-found/authorization error.</returns>
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

        return Result.Success(_mapper.Map<EngineModelDto>(engineModel));
    }
}