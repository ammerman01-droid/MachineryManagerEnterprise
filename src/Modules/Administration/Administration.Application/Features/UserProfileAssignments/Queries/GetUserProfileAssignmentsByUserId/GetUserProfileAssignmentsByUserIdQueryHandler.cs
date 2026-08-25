using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.UserProfileAssignments.Queries.GetUserProfileAssignmentsByUserId;

/// <summary>Handles <see cref="GetUserProfileAssignmentsByUserIdQuery"/>.</summary>
public sealed class GetUserProfileAssignmentsByUserIdQueryHandler
    : IRequestHandler<GetUserProfileAssignmentsByUserIdQuery, Result<IReadOnlyList<UserProfileAssignmentDto>>>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;

    /// <summary>Initializes a new instance of the <see cref="GetUserProfileAssignmentsByUserIdQueryHandler"/> class.</summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    public GetUserProfileAssignmentsByUserIdQueryHandler(IUserProfileAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    /// <summary>Executes the query and returns the user's profile assignments.</summary>
    /// <param name="request">The query containing the user identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the list of assignments.</returns>
    public async Task<Result<IReadOnlyList<UserProfileAssignmentDto>>> Handle(
        GetUserProfileAssignmentsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var assignments = await _assignmentRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var dtos = assignments.Select(a => new UserProfileAssignmentDto(
            a.Id.Value,
            a.UserId,
            a.ProfileId.Value,
            a.Scope.Level.ToString(),
            a.Scope.HoldingId,
            a.Scope.OrganizationId,
            a.Scope.ProjectId,
            a.AssignedAt,
            a.IsActive,
            a.LastChangedAt)).ToList();

        return Result.Success<IReadOnlyList<UserProfileAssignmentDto>>(dtos);
    }
}