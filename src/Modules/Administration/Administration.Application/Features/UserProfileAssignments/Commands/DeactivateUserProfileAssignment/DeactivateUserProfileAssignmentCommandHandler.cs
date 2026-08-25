using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeactivateUserProfileAssignment;

/// <summary>
/// Handles <see cref="DeactivateUserProfileAssignmentCommand"/> by
/// loading the assignment aggregate, invoking domain deactivation, and
/// committing the unit of work.
/// </summary>
public sealed class DeactivateUserProfileAssignmentCommandHandler
    : IRequestHandler<DeactivateUserProfileAssignmentCommand, Result>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeactivateUserProfileAssignmentCommandHandler"/> class.
    /// </summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public DeactivateUserProfileAssignmentCommandHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the deactivation use case.
    /// </summary>
    /// <param name="request">The deactivation command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        DeactivateUserProfileAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignmentId = global::Administration.Domain.UserProfileAssignmentId.From(request.AssignmentId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(
                global::Administration.Domain.ProfileErrors.AssignmentNotFound(request.AssignmentId));
        }

        assignment.Deactivate(_dateTimeProvider);

        _assignmentRepository.Update(assignment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}