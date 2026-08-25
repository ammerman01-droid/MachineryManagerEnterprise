using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeleteUserProfileAssignment;

/// <summary>
/// Handles <see cref="DeleteUserProfileAssignmentCommand"/> by
/// permanently removing the assignment record and committing the unit
/// of work (chat, 2026-08-25 — revised).
/// </summary>
public sealed class DeleteUserProfileAssignmentCommandHandler
    : IRequestHandler<DeleteUserProfileAssignmentCommand, Result>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserProfileAssignmentCommandHandler"/> class.
    /// </summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    public DeleteUserProfileAssignmentCommandHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IAdministrationUnitOfWork unitOfWork)
    {
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Executes the deletion use case.
    /// </summary>
    /// <param name="request">The deletion command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public async Task<Result> Handle(
        DeleteUserProfileAssignmentCommand request,
        CancellationToken cancellationToken)
    {
        var assignmentId = global::Administration.Domain.UserProfileAssignmentId.From(request.AssignmentId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(
                global::Administration.Domain.ProfileErrors.AssignmentNotFound(request.AssignmentId));
        }

        _assignmentRepository.Remove(assignment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}