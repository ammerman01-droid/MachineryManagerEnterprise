using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.SetUserActive;

/// <summary>
/// Handles <see cref="SetUserActiveCommand"/>: needs <c>User.Edit</c> on the user's home and a caller
/// level above (or, for Holding/Platform, equal to) the user's. Nobody may deactivate themselves.
/// Deactivation also ends all of the user's sessions at once.
/// </summary>
public sealed class SetUserActiveCommandHandler : IRequestHandler<SetUserActiveCommand, Result>
{
    private readonly IUserAuthorizationService _authorization;
    private readonly IUserAccountService _accounts;
    private readonly IUserSessionInvalidator _sessionInvalidator;

    /// <summary>Initializes a new instance of the <see cref="SetUserActiveCommandHandler"/> class.</summary>
    /// <param name="authorization">Decides who may manage users.</param>
    /// <param name="accounts">Changes the account state.</param>
    /// <param name="sessionInvalidator">Ends the user's sessions on deactivation.</param>
    public SetUserActiveCommandHandler(
        IUserAuthorizationService authorization,
        IUserAccountService accounts,
        IUserSessionInvalidator sessionInvalidator)
    {
        _authorization = authorization;
        _accounts = accounts;
        _sessionInvalidator = sessionInvalidator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(SetUserActiveCommand request, CancellationToken cancellationToken)
    {
        var actorResult = await _authorization.GetActorAsync(cancellationToken);

        if (actorResult.IsFailure)
        {
            return Result.Failure(actorResult.Error);
        }

        var loaded = await _authorization.LoadUserForManagementAsync(actorResult.Value, request.UserId, "Edit", cancellationToken);

        if (loaded.IsFailure)
        {
            return Result.Failure(loaded.Error);
        }

        if (!request.Active && request.UserId == actorResult.Value.UserId)
        {
            return Result.Failure(Error.Conflict("User.CannotDeactivateSelf", "You cannot deactivate your own account."));
        }

        var result = await _accounts.SetActiveAsync(request.UserId, request.Active, cancellationToken);

        if (result.IsSuccess && !request.Active)
        {
            await _sessionInvalidator.InvalidateAsync(request.UserId, cancellationToken);
        }

        return result;
    }
}
