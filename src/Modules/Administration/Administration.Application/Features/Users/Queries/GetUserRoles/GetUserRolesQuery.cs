using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.GetUserRoles;

/// <summary>Retrieves the Identity roles of one user the caller may see.</summary>
/// <param name="UserId">The user.</param>
public sealed record GetUserRolesQuery(Guid UserId) : IRequest<Result<IReadOnlyList<string>>>;
