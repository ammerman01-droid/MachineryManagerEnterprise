using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.AssetModels.Queries.SearchAssetModels;

/// <summary>
/// Handles <see cref="SearchAssetModelsQuery"/> by verifying the caller
/// is authorized for the requested Holding, then delegating to the
/// repository search projection.
/// </summary>
public sealed class SearchAssetModelsQueryHandler
    : IRequestHandler<SearchAssetModelsQuery, Result<SearchAssetModelsResponse>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IAssetModelRepository _assetModelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="SearchAssetModelsQueryHandler"/> class.</summary>
    public SearchAssetModelsQueryHandler(
        IAssetModelRepository assetModelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _assetModelRepository = assetModelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the search query.</summary>
    public async Task<Result<SearchAssetModelsResponse>> Handle(
        SearchAssetModelsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchAssetModelsResponse>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(request.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<SearchAssetModelsResponse>(global::Asset.Domain.AssetModelErrors.NotAuthorized());
        }

        var response = await _assetModelRepository.SearchAsync(
            request.HoldingId,
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}