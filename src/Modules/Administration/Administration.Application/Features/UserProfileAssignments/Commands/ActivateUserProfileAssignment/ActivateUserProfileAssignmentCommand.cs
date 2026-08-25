using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.ActivateUserProfileAssignment;

/// <summary>
/// Command to (re)activate a UserProfileAssignment that is currently
/// inactive. Automatically deactivates whichever other assignment
/// currently holds the same user's active slot, if any, so that at most
/// one assignment stays active per user (chat, 2026-08-25 — revised).
/// </summary>
/// <param name="AssignmentId">The GUID of the assignment to activate.</param>
public sealed record ActivateUserProfileAssignmentCommand(Guid AssignmentId)
    : IRequest<Result>;