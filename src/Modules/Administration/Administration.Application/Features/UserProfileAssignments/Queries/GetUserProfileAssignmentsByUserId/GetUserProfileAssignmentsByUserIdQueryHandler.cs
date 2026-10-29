using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.UserProfileAssignments.Queries.GetUserProfileAssignmentsByUserId;

/// <summary>
/// Handles <see cref="GetUserProfileAssignmentsByUserIdQuery"/>. A user outside the caller's scope
/// is reported as NotFound; otherwise the caller needs <c>User.View</c> on the user's home.
/// </summary>
public sealed class GetUserProfileAssignmentsByUserIdQueryHandler
    : IRequestHandler<GetUserProfileAssignmentsByUserIdQuery, Result<IReadOnlyList<UserProfileAssignmentDto>>>
{
    private readonly IUserProfileAssignmentRepository _assignmentRepository;
    private readonly IUserAuthorizationService _authorization;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetUserProfileAssignmentsByUserIdQueryHandler"/> class.</summary>
    /// <param name="assignmentRepository">The user-profile assignment repository.</param>
    /// <param name="authorization">Decides which users the caller may see.</param>
    /// <param name="mapper">Maps assignments to their DTOs.</param>
    public GetUserProfileAssignmentsByUserIdQueryHandler(
        IUserProfileAssignmentRepository assignmentRepository,
        IUserAuthorizationService authorization,
        IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _authorization = authorization;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<UserProfileAssignmentDto>>> Handle(
        GetUserProfileAssignmentsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var actor = await _authorization.GetActorAsync(cancellationToken);

        if (actor.IsFailure)
        {
            return Result.Failure<IReadOnlyList<UserProfileAssignmentDto>>(actor.Error);
        }

        var user = await _authorization.LoadUserForViewAsync(actor.Value, request.UserId, cancellationToken);

        if (user.IsFailure)
        {
            return Result.Failure<IReadOnlyList<UserProfileAssignmentDto>>(user.Error);
        }

        var assignments = await _assignmentRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var dtos = _mapper.Map<List<UserProfileAssignmentDto>>(assignments);

        return Result.Success<IReadOnlyList<UserProfileAssignmentDto>>(dtos);
    }
}
