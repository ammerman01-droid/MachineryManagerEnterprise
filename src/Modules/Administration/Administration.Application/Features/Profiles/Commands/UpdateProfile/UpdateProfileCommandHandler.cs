using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.UpdateProfile;

/// <summary>
/// Handles <see cref="UpdateProfileCommand"/> by loading the aggregate,
/// applying the domain update, and committing the unit of work.
/// </summary>
public sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileCommandHandler"/> class.
    /// </summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    public UpdateProfileCommandHandler(
        IProfileRepository profileRepository,
        IAdministrationUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Executes the update use case.
    /// </summary>
    /// <param name="request">The update command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        UpdateProfileCommand request,
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