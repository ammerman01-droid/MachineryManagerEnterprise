using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Queries.GetUserProfileAssignmentsByUserId;

/// <summary>Read-only view of a UserProfileAssignment.</summary>
public sealed record UserProfileAssignmentDto(
    Guid Id,
    Guid UserId,
    Guid ProfileId,
    string ScopeLevel,
    Guid? ScopeHoldingId,
    Guid? ScopeOrganizationId,
    Guid? ScopeProjectId,
    DateTimeOffset AssignedAt,
    bool IsRevoked,
    DateTimeOffset? RevokedAt);

/// <summary>Query to retrieve assignments for a specific user.</summary>
public sealed record GetUserProfileAssignmentsByUserIdQuery(Guid UserId)
    : IRequest<Result<IReadOnlyList<UserProfileAssignmentDto>>>;