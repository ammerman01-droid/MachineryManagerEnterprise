using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.DeleteProfile;

/// <summary>
/// Handles <see cref="DeleteProfileCommand"/>: requires <c>Profile.Delete</c> on the owning
/// scope; system profiles and profiles with active assignments cannot be deleted.
/// </summary>
public sealed class DeleteProfileCommandHandler
    : IRequestHandler<DeleteProfileCommand, Result>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IProfileAuthorizationService _authorization;

    /// <summary>Initializes a new instance of the <see cref="DeleteProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="authorization">Decides who may delete profiles.</param>
    public DeleteProfileCommandHandler(
        IProfileRepository profileRepository,
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IProfileAuthorizationService authorization)
    {
        _profileRepository = profileRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _authorization = authorization;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(
        DeleteProfileCommand request,
        CancellationToken cancellationToken)
    {
        var loaded = await _authorization.GetForManagementAsync(
            request.ProfileId,
            "Delete",
            permissionsToGrant: null,
            cancellationToken);

        if (loaded.IsFailure)
        {
            return Result.Failure(loaded.Error);
        }

        var profile = loaded.Value;

        if (profile.IsSystem)
        {
            return Result.Failure(ProfileErrors.SystemProfileImmutable());
        }

        var profileId = ProfileId.From(request.ProfileId);

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
