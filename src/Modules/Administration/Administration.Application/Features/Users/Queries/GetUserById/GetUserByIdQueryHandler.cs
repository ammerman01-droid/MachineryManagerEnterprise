using MachineryManagerEnterprise.Administration.Application.Features.Users.Dtos;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.GetUserById;

/// <summary>Handles <see cref="GetUserByIdQuery"/>; users outside the caller's scope are reported as NotFound.</summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserAuthorizationService _authorization;

    /// <summary>Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.</summary>
    /// <param name="authorization">Decides which users the caller may see.</param>
    public GetUserByIdQueryHandler(IUserAuthorizationService authorization)
    {
        _authorization = authorization;
    }

    /// <inheritdoc />
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await _authorization.GetActorAsync(cancellationToken);

        if (actor.IsFailure)
        {
            return Result.Failure<UserDto>(actor.Error);
        }

        var loaded = await _authorization.LoadUserForViewAsync(actor.Value, request.UserId, cancellationToken);

        return loaded.IsFailure
            ? Result.Failure<UserDto>(loaded.Error)
            : new UserDto(loaded.Value.Account.Id, loaded.Value.Account.UserName, loaded.Value.Account.IsActive);
    }
}
