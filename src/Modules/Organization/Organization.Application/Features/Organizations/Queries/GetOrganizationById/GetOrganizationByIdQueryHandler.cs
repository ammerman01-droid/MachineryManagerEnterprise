using MachineryManagerEnterprise.Organization.Application.Abstractions;
using MachineryManagerEnterprise.Organization.Application.Features.Organizations.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MapsterMapper;
using MediatR;
using Organization.Domain;

namespace MachineryManagerEnterprise.Organization.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Handles <see cref="GetOrganizationByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetOrganizationByIdQueryHandler
    : IRequestHandler<GetOrganizationByIdQuery, Result<OrganizationDto>>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrganizationByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="organizationRepository">The organization repository.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetOrganizationByIdQueryHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Executes the query and returns the organization DTO if found.
    /// </summary>
    /// <param name="request">The query containing the organization identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the <see cref="OrganizationDto"/> or a not-found error.</returns>
    public async Task<Result<OrganizationDto>> Handle(
        GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var organizationId = OrganizationId.From(request.OrganizationId);
        var organization = await _organizationRepository.GetByIdAsync(
            organizationId,
            cancellationToken);

        if (organization is null)
        {
            return Result.Failure<OrganizationDto>(
                Error.NotFound(
                    "Organization.NotFound",
                    $"Organization with id {request.OrganizationId} was not found."));
        }

        return Result.Success(_mapper.Map<OrganizationDto>(organization));
    }
}