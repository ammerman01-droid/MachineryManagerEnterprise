using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.Administration.Application.Features.Profiles.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Queries.GetProfileById;

/// <summary>
/// Handles <see cref="GetProfileByIdQuery"/>. A profile the caller may not see is reported
/// as NotFound, so other tenants' profiles are never revealed.
/// </summary>
public sealed class GetProfileByIdQueryHandler
    : IRequestHandler<GetProfileByIdQuery, Result<ProfileDto>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IProfileAuthorizationService _authorization;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetProfileByIdQueryHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="authorization">Decides which profiles the caller may see.</param>
    /// <param name="mapper">Maps the aggregate to its DTO.</param>
    public GetProfileByIdQueryHandler(
        IProfileRepository profileRepository,
        IProfileAuthorizationService authorization,
        IMapper mapper)
    {
        _profileRepository = profileRepository;
        _authorization = authorization;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<ProfileDto>> Handle(
        GetProfileByIdQuery request,
        CancellationToken cancellationToken)
    {
        var visibility = await _authorization.ResolveVisibilityAsync(cancellationToken);

        if (visibility.IsFailure)
        {
            return Result.Failure<ProfileDto>(visibility.Error);
        }

        var profileId = global::Administration.Domain.ProfileId.From(request.ProfileId);
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);

        if (profile is null || !visibility.Value.CanSee(profile.HoldingId))
        {
            return Result.Failure<ProfileDto>(
                Error.NotFound(
                    "Profile.NotFound",
                    $"Profile with id {request.ProfileId} was not found."));
        }

        return Result.Success(_mapper.Map<ProfileDto>(profile));
    }
}
