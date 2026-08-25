using Administration.Domain;
using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.DeleteProfile;

/// <summary>
/// Handles <see cref="DeleteProfileCommand"/> by verifying the profile
/// has no active assignments, then permanently removing it along with
/// any of its (already-revoked) assignment records — so no orphaned
/// UserProfileAssignment rows are left behind (chat, 2026-08-25).
/// </summary>
public sealed class DeleteProfileCommandHandler
    : IRequestHandler<DeleteProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;

    /// <summary>Initializes a new instance of the <see cref="DeleteProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    public DeleteProfileCommandHandler(
        IProfileRepository profileRepository,
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Executes the delete use case.
    /// </summary>
    /// <param name="request">The delete command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A result indicating success, a Not Found error if the profile
    /// does not exist, or a Conflict error (with a message the UI can
    /// surface to the admin) if the profile is still assigned to users.
    /// </returns>
    public async Task<Result> Handle(
        DeleteProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profileId = ProfileId.From(request.ProfileId);
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure(ProfileErrors.ProfileNotFound(request.ProfileId));
        }

        var hasActiveAssignments = await _assignmentRepository.HasActiveAssignmentsForProfileAsync(
            profileId,
            cancellationToken);

        if (hasActiveAssignments)
        {
            return Result.Failure(ProfileErrors.ProfileHasActiveAssignments());
        }

        // Cascade cleanup: any remaining assignments at this point are
        // necessarily revoked (active ones were blocked above). Remove
        // them so no orphaned rows reference a Profile that is about to
        // no longer exist.
        await _assignmentRepository.RemoveAllForProfileAsync(profileId, cancellationToken);

        _profileRepository.Remove(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}