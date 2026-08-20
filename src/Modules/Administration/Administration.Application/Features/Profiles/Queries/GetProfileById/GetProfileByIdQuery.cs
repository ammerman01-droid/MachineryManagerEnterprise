using MachineryManager.Administration.Application.Features.Profiles.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Queries.GetProfileById;

/// <summary>
/// Query to retrieve a single profile by its unique identifier.
/// </summary>
/// <param name="ProfileId">The GUID of the profile to retrieve.</param>
public sealed record GetProfileByIdQuery(Guid ProfileId)
    : IRequest<Result<ProfileDto>>;