using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.ActivateUserProfileAssignment;

/// <summary>
/// Handles <see cref="ActivateUserProfileAssignmentCommand"/> by
/// deactivating the user's currently active assignment (if it differs
/// from the target), activating the target assignment, and committing
/// the unit of work (chat, 2026-08-25 — revised).
/// </summary>
public sealed class ActivateUserProfileAssignmentCommandHandler
    : IRequestHandler<ActivateUserProfileAssignmentCommand, Result>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivateUserProfileAssignmentCommandHandler"/> class.
    /// </summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public ActivateUserProfileAssignmentCommandHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the activation use case.
    /// </summary>
    /// <param name="request">The activation command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        ActivateUserProfileAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignmentId = global::Administration.Domain.UserProfileAssignmentId.From(request.AssignmentId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(
                global::Administration.Domain.ProfileErrors.AssignmentNotFound(request.AssignmentId));
        }

        // Deactivate whichever OTHER assignment currently holds this
        // user's active slot, so at most one stays active afterward.
        var currentlyActive = await _assignmentRepository.GetActiveByUserIdAsync(
            assignment.UserId,
            cancellationToken);

        foreach (var existing in currentlyActive)
        {
            if (existing.Id != assignment.Id)
            {
                existing.Deactivate(_dateTimeProvider);
                _assignmentRepository.Update(existing);
            }
        }

        assignment.Activate(_dateTimeProvider);
        _assignmentRepository.Update(assignment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}