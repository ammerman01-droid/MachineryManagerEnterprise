using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.DeleteProfile;

/// <summary>
/// Command to permanently delete a Profile. Blocked while the profile
/// still has any active (non-revoked) UserProfileAssignment (chat,
/// 2026-08-25).
/// </summary>
/// <param name="ProfileId">The unique identifier of the profile to delete.</param>
public sealed record DeleteProfileCommand(Guid ProfileId)
    : IRequest<Result>;
