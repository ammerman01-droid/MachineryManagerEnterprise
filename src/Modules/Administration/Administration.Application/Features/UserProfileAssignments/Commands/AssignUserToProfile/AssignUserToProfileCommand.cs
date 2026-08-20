using Administration.Domain;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.AssignUserToProfile;

/// <summary>
/// Command to assign a User to a Profile at a specific authorization scope.
/// </summary>
/// <param name="UserId">The GUID of the user.</param>
/// <param name="ProfileId">The GUID of the profile.</param>
/// <param name="Scope">The authorization scope.</param>
public sealed record AssignUserToProfileCommand(
    Guid UserId,
    Guid ProfileId,
    AuthorizationScope Scope)
    : IRequest<Result<Guid>>;