using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.GetProfileById;

/// <summary>
/// Handles <see cref="GetProfileByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetProfileByIdQueryHandler
    : IRequestHandler<GetProfileByIdQuery, Result<ProfileDto>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProfileByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetProfileByIdQueryHandler(IProfileRepository profileRepository, IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Executes the query and returns the profile DTO if found.
    /// </summary>
    /// <param name="request">The query containing the profile identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="ProfileDto"/> or a not-found error.</returns>
    public async Task<Result<ProfileDto>> Handle(
        GetProfileByIdQuery request,
        CancellationToken cancellationToken)
    {
        var profileId = global::Administration.Domain.ProfileId.From(request.ProfileId);
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            return Result.Failure<ProfileDto>(
                Error.NotFound(
                    "Profile.NotFound",
                    $"Profile with id {request.ProfileId} was not found."));
        }

        return Result.Success(_mapper.Map<ProfileDto>(profile));
    }
}