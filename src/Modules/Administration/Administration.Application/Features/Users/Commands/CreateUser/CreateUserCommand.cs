using Administration.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Creates a user account and, when a scope is given, atomically gives it a home in the
/// hierarchy and its first profile assignment.
/// </summary>
/// <param name="UserName">The sign-in name.</param>
/// <param name="Password">The initial password.</param>
/// <param name="Scope">
/// The scope the user will work in. <see langword="null"/> creates a bare Platform account
/// (Platform callers only) that is assigned a profile separately.
/// </param>
/// <param name="ProfileId">
/// The profile to assign. Required when a scope is given, except for the first Holding-level
/// user of a Holding, who always receives the system-managed full-access profile.
/// </param>
/// <param name="Roles">Identity roles to grant. Honoured for Platform callers only.</param>
public sealed record CreateUserCommand(
    string UserName,
    string Password,
    AuthorizationScope? Scope = null,
    Guid? ProfileId = null,
    IReadOnlyList<string>? Roles = null)
    : IRequest<Result<Guid>>;
