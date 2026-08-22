using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.DeactivateProfile;

/// <summary>
/// Command to deactivate a Profile, preventing new assignments.
/// </summary>
/// <param name="ProfileId">The unique identifier of the profile to deactivate.</param>
public sealed record DeactivateProfileCommand(Guid ProfileId)
    : IRequest<Result>;