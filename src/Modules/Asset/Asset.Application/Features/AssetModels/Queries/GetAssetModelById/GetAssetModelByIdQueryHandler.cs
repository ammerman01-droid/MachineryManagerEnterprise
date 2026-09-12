using MachineryManagerEnterprise.Asset.Application.Abstractions;
using MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.AssetModels.Queries.GetAssetModelById;

/// <summary>
/// Handles <see cref="GetAssetModelByIdQuery"/> by loading the
/// aggregate, verifying the caller is authorized for its Holding, and
/// mapping it to a DTO.
/// </summary>
public sealed class GetAssetModelByIdQueryHandler
    : IRequestHandler<GetAssetModelByIdQuery, Result<AssetModelDto>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetAssetModelByIdQueryHandler"/> class.</summary>
    /// <param name="assetModelRepository">The Asset Model repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetAssetModelByIdQueryHandler(
        IAssetModelRepository assetModelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _assetModelRepository = assetModelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query containing the Asset Model identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="AssetModelDto"/> or a not-found/authorization error.</returns>
    public async Task<Result<AssetModelDto>> Handle(GetAssetModelByIdQuery request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.AssetModelId.From(request.AssetModelId);
        var assetModel = await _assetModelRepository.GetByIdAsync(id, cancellationToken);

        if (assetModel is null)
        {
            return Result.Failure<AssetModelDto>(
                Error.NotFound("AssetModel.NotFound", $"Asset model with id {request.AssetModelId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<AssetModelDto>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(assetModel.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<AssetModelDto>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        return Result.Success(_mapper.Map<AssetModelDto>(assetModel));
    }
}