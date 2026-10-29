using MachineryManagerEnterprise.Administration.Application.Features.Access.Dtos;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Access.Queries.GetMyAccess;

/// <summary>Handles <see cref="GetMyAccessQuery"/>.</summary>
public sealed class GetMyAccessQueryHandler : IRequestHandler<GetMyAccessQuery, Result<MyAccessDto>>
{
    private readonly IUserAuthorizationService _authorization;
    private readonly IEffectivePermissionsProvider _effectivePermissions;

    /// <summary>Initializes a new instance of the <see cref="GetMyAccessQueryHandler"/> class.</summary>
    /// <param name="authorization">Resolves the caller's scope.</param>
    /// <param name="effectivePermissions">Computes the caller's effective permissions.</param>
    public GetMyAccessQueryHandler(
        IUserAuthorizationService authorization,
        IEffectivePermissionsProvider effectivePermissions)
    {
        _authorization = authorization;
        _effectivePermissions = effectivePermissions;
    }

    /// <inheritdoc />
    public async Task<Result<MyAccessDto>> Handle(GetMyAccessQuery request, CancellationToken cancellationToken)
    {
        var actorResult = await _authorization.GetActorAsync(cancellationToken);

        if (actorResult.IsFailure)
        {
            return Result.Failure<MyAccessDto>(actorResult.Error);
        }

        var actor = actorResult.Value;
        var scope = new ScopeChain(actor.Level, actor.HoldingId, actor.OrganizationId, actor.ProjectId);
        var effective = await _effectivePermissions.GetAsync(actor.UserId, scope.ToResourceScope(), cancellationToken);

        return new MyAccessDto(
            actor.Level.ToString(),
            actor.HoldingId,
            actor.OrganizationId,
            actor.ProjectId,
            effective.HasWildcard,
            effective.Permissions.OrderBy(p => p, StringComparer.Ordinal).ToList());
    }
}
