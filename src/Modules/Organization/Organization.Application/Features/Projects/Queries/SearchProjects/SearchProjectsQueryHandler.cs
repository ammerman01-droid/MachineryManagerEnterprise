using MachineryManagerEnterprise.Organization.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManagerEnterprise.Organization.Application.Features.Projects.Queries.SearchProjects;

/// <summary>
/// Handles <see cref="SearchProjectsQuery"/> by delegating to the repository with the
/// caller's authorized scope. The caller's own Project is always included so that context
/// pickers work for users who work below that level.
/// </summary>
public sealed class SearchProjectsQueryHandler
    : IRequestHandler<SearchProjectsQuery, Result<SearchProjectsResponse>>
{
    private const string RequiredPermission = "Project.View";

    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IUserAuthorizationScopeResolver _scopeResolver;

    /// <summary>Initializes a new instance of the <see cref="SearchProjectsQueryHandler"/> class.</summary>
    /// <param name="projectRepository">The Project repository.</param>
    /// <param name="currentUserService">Identifies the caller.</param>
    /// <param name="permissionEvaluator">Evaluates the caller's authorized scope.</param>
    /// <param name="scopeResolver">Resolves the caller's own position in the tenant tree.</param>
    public SearchProjectsQueryHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IUserAuthorizationScopeResolver scopeResolver)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _scopeResolver = scopeResolver;
    }

    /// <inheritdoc />
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

        var ownScope = await _scopeResolver.ResolveAsync(userId, cancellationToken);

        var response = await _projectRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            authorizedScope.IncludingOwnProject(ownScope),
            cancellationToken);

        return Result.Success(response);
    }
}
