using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.ActivateProfile;

/// <summary>
/// Command to activate a previously deactivated Profile.
/// </summary>
/// <param name="ProfileId">The unique identifier of the profile to activate.</param>
public sealed record ActivateProfileCommand(Guid ProfileId)
    : IRequest<Result>;