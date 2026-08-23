using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.RevokeUserProfileAssignment;

/// <summary>
/// Handles <see cref="RevokeUserProfileAssignmentCommand"/> by loading the
/// assignment aggregate, invoking domain revocation, and committing the
/// unit of work (BR-017, Access revocation on reassignment).
/// </summary>
public sealed class RevokeUserProfileAssignmentCommandHandler
    : IRequestHandler<RevokeUserProfileAssignmentCommand, Result>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RevokeUserProfileAssignmentCommandHandler"/> class.
    /// </summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public RevokeUserProfileAssignmentCommandHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the revocation use case.
    /// </summary>
    /// <param name="request">The revocation command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        RevokeUserProfileAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignmentId = global::Administration.Domain.UserProfileAssignmentId.From(request.AssignmentId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(
                global::Administration.Domain.ProfileErrors.AssignmentNotFound(request.AssignmentId));
        }

        var result = assignment.Revoke(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _assignmentRepository.Update(assignment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}