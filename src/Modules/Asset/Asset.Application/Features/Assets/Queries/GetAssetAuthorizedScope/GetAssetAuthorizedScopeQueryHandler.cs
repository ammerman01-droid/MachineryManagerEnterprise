using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Queries.GetAssetAuthorizedScope;

/// <summary>Handles <see cref="GetAssetAuthorizedScopeQuery"/>.</summary>
public sealed class GetAssetAuthorizedScopeQueryHandler
    : IRequestHandler<GetAssetAuthorizedScopeQuery, Result<AuthorizedScopeSet>>
{
    private const string RequiredPermission = "Asset.View";

    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetAssetAuthorizedScopeQueryHandler"/> class.</summary>
    public GetAssetAuthorizedScopeQueryHandler(
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the query.</summary>
    public async Task<Result<AuthorizedScopeSet>> Handle(
        GetAssetAuthorizedScopeQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<AuthorizedScopeSet>(global::Asset.Domain.AssetErrors.NotAuthorized());
        }

        var scope = await _permissionEvaluator.GetAuthorizedScopesAsync(userId, RequiredPermission, cancellationToken);

        return Result.Success(scope);
    }
}