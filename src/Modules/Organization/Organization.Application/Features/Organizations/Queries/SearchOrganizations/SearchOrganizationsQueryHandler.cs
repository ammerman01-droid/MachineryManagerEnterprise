using MachineryManagerEnterprise.Organization.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManagerEnterprise.Organization.Application.Features.Organizations.Queries.SearchOrganizations;

/// <summary>
/// Handles <see cref="SearchOrganizationsQuery"/> by delegating to the repository with the
/// caller's authorized scope. The caller's own Organization is always included so that context
/// pickers work for users who work below that level.
/// </summary>
public sealed class SearchOrganizationsQueryHandler
    : IRequestHandler<SearchOrganizationsQuery, Result<SearchOrganizationsResponse>>
{
    private const string RequiredPermission = "Organization.View";

    private readonly IOrganizationRepository _organizationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IUserAuthorizationScopeResolver _scopeResolver;

    /// <summary>Initializes a new instance of the <see cref="SearchOrganizationsQueryHandler"/> class.</summary>
    /// <param name="organizationRepository">The Organization repository.</param>
    /// <param name="currentUserService">Identifies the caller.</param>
    /// <param name="permissionEvaluator">Evaluates the caller's authorized scope.</param>
    /// <param name="scopeResolver">Resolves the caller's own position in the tenant tree.</param>
    public SearchOrganizationsQueryHandler(
        IOrganizationRepository organizationRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IUserAuthorizationScopeResolver scopeResolver)
    {
        _organizationRepository = organizationRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _scopeResolver = scopeResolver;
    }

    /// <inheritdoc />
    public async Task<Result<SearchOrganizationsResponse>> Handle(
        SearchOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchOrganizationsResponse>(OrganizationErrors.NotAuthorized());
        }

        var authorizedScope = await _permissionEvaluator.GetAuthorizedScopesAsync(
            userId,
            RequiredPermission,
            cancellationToken);

        var ownScope = await _scopeResolver.ResolveAsync(userId, cancellationToken);

        var response = await _organizationRepository.SearchAsync(
            request.SearchTerm,
            request.HoldingId,
            request.Page,
            request.PageSize,
            authorizedScope.IncludingOwnOrganization(ownScope),
            cancellationToken);

        return Result.Success(response);
    }
}
