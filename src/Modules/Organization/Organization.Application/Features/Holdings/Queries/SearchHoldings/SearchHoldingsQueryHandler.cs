using MachineryManagerEnterprise.Organization.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;

namespace MachineryManagerEnterprise.Organization.Application.Features.Holdings.Queries.SearchHoldings;

/// <summary>
/// Handles <see cref="SearchHoldingsQuery"/> by delegating to the repository with the
/// caller's authorized scope. The caller's own Holding is always included so that context
/// pickers work for users who work below that level.
/// </summary>
public sealed class SearchHoldingsQueryHandler
    : IRequestHandler<SearchHoldingsQuery, Result<SearchHoldingsResponse>>
{
    private const string RequiredPermission = "Holding.View";

    private readonly IHoldingRepository _holdingRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IUserAuthorizationScopeResolver _scopeResolver;

    /// <summary>Initializes a new instance of the <see cref="SearchHoldingsQueryHandler"/> class.</summary>
    /// <param name="holdingRepository">The Holding repository.</param>
    /// <param name="currentUserService">Identifies the caller.</param>
    /// <param name="permissionEvaluator">Evaluates the caller's authorized scope.</param>
    /// <param name="scopeResolver">Resolves the caller's own position in the tenant tree.</param>
    public SearchHoldingsQueryHandler(
        IHoldingRepository holdingRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IUserAuthorizationScopeResolver scopeResolver)
    {
        _holdingRepository = holdingRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _scopeResolver = scopeResolver;
    }

    /// <inheritdoc />
    public async Task<Result<SearchHoldingsResponse>> Handle(
        SearchHoldingsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<SearchHoldingsResponse>(HoldingErrors.NotAuthorized());
        }

        var authorizedScope = await _permissionEvaluator.GetAuthorizedScopesAsync(
            userId,
            RequiredPermission,
            cancellationToken);

        var ownScope = await _scopeResolver.ResolveAsync(userId, cancellationToken);

        var response = await _holdingRepository.SearchAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            authorizedScope.IncludingOwnHolding(ownScope),
            cancellationToken);

        return Result.Success(response);
    }
}
