using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Queries.GetLubricantTypeById;

/// <summary>Retrieves a single Lubricant Type by its identifier.</summary>
public sealed record GetLubricantTypeByIdQuery(Guid LubricantTypeId) : IRequest<Result<LubricantTypeDto>>;
