using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.GetUserRoles;

/// <summary>Handles <see cref="GetUserRolesQuery"/>; users outside the caller's scope are reported as NotFound.</summary>
public sealed class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<IReadOnlyList<string>>>
{
    private readonly IUserAuthorizationService _authorization;
    private readonly IUserAccountService _accounts;

    /// <summary>Initializes a new instance of the <see cref="GetUserRolesQueryHandler"/> class.</summary>
    /// <param name="authorization">Decides which users the caller may see.</param>
    /// <param name="accounts">Reads the roles.</param>
    public GetUserRolesQueryHandler(IUserAuthorizationService authorization, IUserAccountService accounts)
    {
        _authorization = authorization;
        _accounts = accounts;
    }

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var actor = await _authorization.GetActorAsync(cancellationToken);

        if (actor.IsFailure)
        {
            return Result.Failure<IReadOnlyList<string>>(actor.Error);
        }

        var loaded = await _authorization.LoadUserForViewAsync(actor.Value, request.UserId, cancellationToken);

        if (loaded.IsFailure)
        {
            return Result.Failure<IReadOnlyList<string>>(loaded.Error);
        }

        return Result.Success(await _accounts.GetRolesAsync(request.UserId, cancellationToken));
    }
}
