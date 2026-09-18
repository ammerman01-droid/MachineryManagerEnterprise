using MachineryManagerEnterprise.Personnel.Application.Features.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Queries.GetPersonnelByOrganization;

/// <summary>Retrieves every Personnel record owned by a given Organization.</summary>
public sealed record GetPersonnelByOrganizationQuery(Guid OrganizationId) : IRequest<Result<IReadOnlyList<PersonnelDto>>>;