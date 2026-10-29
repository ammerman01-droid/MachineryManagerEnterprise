using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.DeactivateProfile;

/// <summary>
/// Handles <see cref="DeactivateProfileCommand"/>: requires <c>Profile.Edit</c> on the owning
/// scope; system profiles cannot be changed.
/// </summary>
public sealed class DeactivateProfileCommandHandler
    : IRequestHandler<DeactivateProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IProfileAuthorizationService _authorization;

    /// <summary>Initializes a new instance of the <see cref="DeactivateProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="authorization">Decides who may edit profiles.</param>
    public DeactivateProfileCommandHandler(
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
        DeactivateProfileCommand request,
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
        var result = profile.Deactivate();

        if (result.IsFailure)
        {
            return result;
        }

        _profileRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
