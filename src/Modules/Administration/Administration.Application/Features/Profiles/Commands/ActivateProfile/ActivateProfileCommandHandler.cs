using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.ActivateProfile;

/// <summary>
/// Handles <see cref="ActivateProfileCommand"/> by loading the aggregate,
/// invoking domain activation, and committing the unit of work.
/// </summary>
public sealed class ActivateProfileCommandHandler
    : IRequestHandler<ActivateProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivateProfileCommandHandler"/> class.
    /// </summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    public ActivateProfileCommandHandler(
        IProfileRepository profileRepository,
        IAdministrationUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Executes the activation use case.
    /// </summary>
    /// <param name="request">The activation command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        ActivateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profileId = global::Administration.Domain.ProfileId.From(request.ProfileId);
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure(
                Error.NotFound(
                    "Profile.NotFound",
                    $"Profile with id {request.ProfileId} was not found."));
        }

        profile.Activate();
        _profileRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}