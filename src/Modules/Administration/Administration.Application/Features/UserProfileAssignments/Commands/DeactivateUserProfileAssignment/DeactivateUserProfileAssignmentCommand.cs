using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeactivateUserProfileAssignment;

/// <summary>
/// Command to deactivate an existing UserProfileAssignment. The record
/// stays in the user's assignment list and can be reactivated later via
/// <c>ActivateUserProfileAssignmentCommand</c> (chat, 2026-08-25 —
/// revised: renamed from "Revoke" now that deactivation is reversible).
/// </summary>
/// <param name="AssignmentId">The GUID of the assignment to deactivate.</param>
public sealed record DeactivateUserProfileAssignmentCommand(Guid AssignmentId)
    : IRequest<Result>;