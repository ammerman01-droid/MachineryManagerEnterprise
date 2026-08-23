using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.RevokeUserProfileAssignment;

/// <summary>
/// Command to revoke an existing UserProfileAssignment, immediately
/// removing the User's access at that scope (BR-017).
/// </summary>
/// <param name="AssignmentId">The GUID of the assignment to revoke.</param>
public sealed record RevokeUserProfileAssignmentCommand(Guid AssignmentId)
    : IRequest<Result>;