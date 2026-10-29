using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.UpdateProfile;

/// <summary>
/// Handles <see cref="UpdateProfileCommand"/>: the profile must be visible to the caller,
/// who needs <c>Profile.Edit</c> on the owning scope and may only grant permissions they hold.
/// System profiles are immutable.
/// </summary>
public sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IProfileAuthorizationService _authorization;

    /// <summary>Initializes a new instance of the <see cref="UpdateProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="authorization">Decides who may edit profiles.</param>
    public UpdateProfileCommandHandler(
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
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var loaded = await _authorization.GetForManagementAsync(
            request.ProfileId,
            "Edit",
            request.Permissions,
            cancellationToken);

        if (loaded.IsFailure)
        {
            return Result.Failure(loaded.Error);
        }

        var profile = loaded.Value;
        var result = profile.UpdateInformation(request.Name, request.Permissions);

        if (result.IsFailure)
        {
            return result;
        }

        _profileRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
