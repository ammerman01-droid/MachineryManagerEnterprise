using MachineryManager.Organization.Application.Features.Organizations.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Query to retrieve a single organization by its unique identifier.
/// </summary>
/// <param name="OrganizationId">The GUID of the organization to retrieve.</param>
public sealed record GetOrganizationByIdQuery(Guid OrganizationId)
    : IRequest<Result<OrganizationDto>>;