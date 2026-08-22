using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.UpdateProfile;

/// <summary>
/// Command to update an existing Profile's name and permission set.
/// </summary>
/// <param name="ProfileId">The unique identifier of the profile to update.</param>
/// <param name="Name">The new display name of the profile.</param>
/// <param name="Permissions">The replacement set of permissions.</param>
public sealed record UpdateProfileCommand(
    Guid ProfileId,
    string Name,
    IReadOnlyList<string> Permissions)
    : IRequest<Result>;