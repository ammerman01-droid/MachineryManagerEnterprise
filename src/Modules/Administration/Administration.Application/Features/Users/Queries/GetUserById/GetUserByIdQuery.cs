using MachineryManagerEnterprise.Administration.Application.Features.Users.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.GetUserById;

/// <summary>Retrieves one user the caller may see.</summary>
/// <param name="UserId">The user.</param>
public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;
