using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.Organization.Application.Features.Organizations.Dtos;
using MachineryManager.SharedKernel;
using MediatR;
using Organization.Domain;

namespace MachineryManager.Organization.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Handles <see cref="GetOrganizationByIdQuery"/> by loading the aggregate
/// and projecting it into a read-only DTO.
/// </summary>
public sealed class GetOrganizationByIdQueryHandler
    : IRequestHandler<GetOrganizationByIdQuery, Result<OrganizationDto>>
{
    private readonly IOrganizationRepository _organizationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrganizationByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="organizationRepository">The organization repository.</param>
    public GetOrganizationByIdQueryHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
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

        var dto = new OrganizationDto(
            organization.Id.Value,
            organization.Name,
            organization.IsSuspended);

        return Result.Success(dto);
    }
}