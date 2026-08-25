using Administration.Domain;
using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.AssignUserToProfile;

/// <summary>
/// Handles <see cref="AssignUserToProfileCommand"/> by creating the
/// assignment aggregate, persisting it, and committing the unit of work.
/// </summary>
public sealed class AssignUserToProfileCommandHandler
    : IRequestHandler<AssignUserToProfileCommand, Result<Guid>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignUserToProfileCommandHandler"/> class.
    /// </summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public AssignUserToProfileCommandHandler(
        IProfileRepository profileRepository,
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _profileRepository = profileRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the assignment use case.
    /// </summary>
    /// <param name="request">The assignment command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new assignment's GUID on success.</returns>
    public async Task<Result<Guid>> Handle(
        AssignUserToProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profileId = ProfileId.From(request.ProfileId);
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure<Guid>(
                Error.NotFound(
                    "Profile.NotFound",
                    $"Profile with id {request.ProfileId} was not found."));
        }

        if (!profile.IsActive)
        {
            return Result.Failure<Guid>(
                Error.Conflict(
                    "Profile.Inactive",
                    "Cannot assign an inactive profile to a user."));
        }

        // A user may hold at most one active Profile at a time (chat,
        // 2026-08-25). This single check also prevents the same Profile
        // from being assigned twice to the same user, since a duplicate
        // assignment is a specific case of "already has an active
        // assignment".
        var existingActiveAssignments = await _assignmentRepository.GetActiveByUserIdAsync(
            request.UserId,
            cancellationToken);

        if (existingActiveAssignments.Count > 0)
        {
            return Result.Failure<Guid>(
                ProfileErrors.UserAlreadyHasActiveAssignment(existingActiveAssignments[0].ProfileId.Value));
        }

        var result = UserProfileAssignment.Create(
            request.UserId,
            profileId,
            request.Scope,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _assignmentRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}