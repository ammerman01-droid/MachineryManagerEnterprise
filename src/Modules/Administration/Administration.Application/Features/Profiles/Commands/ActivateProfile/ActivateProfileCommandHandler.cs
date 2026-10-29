using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.ActivateProfile;

/// <summary>
/// Handles <see cref="ActivateProfileCommand"/>: requires <c>Profile.Edit</c> on the owning
/// scope; system profiles cannot be changed.
/// </summary>
public sealed class ActivateProfileCommandHandler
    : IRequestHandler<ActivateProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IProfileAuthorizationService _authorization;

    /// <summary>Initializes a new instance of the <see cref="ActivateProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="authorization">Decides who may edit profiles.</param>
    public ActivateProfileCommandHandler(
        IProfileRepository profileRepository,
        IAdministrationUnitOfWork unitOfWork,
        IProfileAuthorizationService authorization)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _authorization = authorization;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(
        ActivateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var loaded = await _authorization.GetForManagementAsync(
            request.ProfileId,
            "Edit",
            permissionsToGrant: null,
            cancellationToken);

        if (loaded.IsFailure)
        {
            return Result.Failure(loaded.Error);
        }

        var profile = loaded.Value;
        var result = profile.Activate();

        if (result.IsFailure)
        {
            return result;
        }

        _profileRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
