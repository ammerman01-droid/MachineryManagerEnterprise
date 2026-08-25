using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;

/// <summary>
/// Handles <see cref="SearchProjectsQuery"/> by resolving the current
/// user's authorized scope and delegating to the repository search
/// projection, restricted to that scope (Phase 3 — Scope-based Filtering).
/// </summary>
public sealed class SearchProjectsQueryHandler
    : IRequestHandler<SearchProjectsQuery, Result<SearchProjectsResponse>>
{
    private const string RequiredPermission = "Project.View";

    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchProjectsQueryHandler"/> class.
    /// </summary>
    /// <param name="projectRepository">The project repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorized scopes at request time.</param>
    public SearchProjectsQueryHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>
    /// Executes the search query, restricted to the current user's authorized scope.
    /// </summary>
    /// <param name="request">The search query parameters.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the paginated search response.</returns>
    public async Task<Result<SearchProjectsResponse>> Handle(
        SearchProjectsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchProjectsResponse>(ProjectErrors.NotAuthorized());
        }

        var authorizedScope = await _permissionEvaluator.GetAuthorizedScopesAsync(
            userId,
            RequiredPermission,
            cancellationToken);

        var response = await _projectRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            authorizedScope,
            cancellationToken);

        return Result.Success(response);
    }
}