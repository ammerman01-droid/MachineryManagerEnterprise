using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.CreateProfile;

/// <summary>
/// Command to create a new Profile (named permission bundle).
/// </summary>
/// <param name="Name">The display name of the profile.</param>
/// <param name="Permissions">The initial set of permissions.</param>
public sealed record CreateProfileCommand(
    string Name,
    IReadOnlyList<string> Permissions)
    : IRequest<Result<Guid>>;