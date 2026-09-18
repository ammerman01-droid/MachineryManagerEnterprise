using MachineryManagerEnterprise.Personnel.Application.Features.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Queries.GetPersonnelById;

/// <summary>Retrieves a single Personnel record by its identifier.</summary>
public sealed record GetPersonnelByIdQuery(Guid PersonnelId) : IRequest<Result<PersonnelDto>>;