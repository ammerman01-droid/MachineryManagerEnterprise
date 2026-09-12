using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.UserProfileAssignments.Queries.GetUserProfileAssignmentsByUserId;

/// <summary>Handles <see cref="GetUserProfileAssignmentsByUserIdQuery"/>.</summary>
public sealed class GetUserProfileAssignmentsByUserIdQueryHandler
    : IRequestHandler<GetUserProfileAssignmentsByUserIdQuery, Result<IReadOnlyList<UserProfileAssignmentDto>>>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetUserProfileAssignmentsByUserIdQueryHandler"/> class.</summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public GetUserProfileAssignmentsByUserIdQueryHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
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

        var dtos = _mapper.Map<List<UserProfileAssignmentDto>>(assignments);

        return Result.Success<IReadOnlyList<UserProfileAssignmentDto>>(dtos);
    }
}