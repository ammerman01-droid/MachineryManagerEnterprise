using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Queries.SearchEngineModels;

/// <summary>
/// Handles <see cref="SearchEngineModelsQuery"/> by verifying the
/// caller is authorized for the requested Holding, then delegating to
/// the repository search projection.
/// </summary>
public sealed class SearchEngineModelsQueryHandler
    : IRequestHandler<SearchEngineModelsQuery, Result<SearchEngineModelsResponse>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly IEngineModelRepository _engineModelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="SearchEngineModelsQueryHandler"/> class.</summary>
    public SearchEngineModelsQueryHandler(
        IEngineModelRepository engineModelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _engineModelRepository = engineModelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the search query.</summary>
    public async Task<Result<SearchEngineModelsResponse>> Handle(
        SearchEngineModelsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchEngineModelsResponse>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(request.HoldingId, null, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<SearchEngineModelsResponse>(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var response = await _engineModelRepository.SearchAsync(
            request.HoldingId,
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result.Success(response);
    }
}