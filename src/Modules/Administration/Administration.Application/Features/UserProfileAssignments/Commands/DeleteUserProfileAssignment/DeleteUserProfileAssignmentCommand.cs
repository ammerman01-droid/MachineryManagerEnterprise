using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeleteUserProfileAssignment;

/// <summary>
/// Command to permanently remove a UserProfileAssignment from a user's
/// assignment list (chat, 2026-08-25 — revised). Unlike Deactivate, this
/// is not reversible: the row is physically deleted and will no longer
/// appear in that user's list at all.
/// </summary>
/// <param name="AssignmentId">The GUID of the assignment to delete.</param>
public sealed record DeleteUserProfileAssignmentCommand(Guid AssignmentId)
    : IRequest<Result>;