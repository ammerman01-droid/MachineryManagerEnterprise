using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Queries.GetLubricantTypesByHolding;

/// <summary>Retrieves the list of Lubricant Type options defined for a Holding.</summary>
public sealed record GetLubricantTypesByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<LubricantTypeDto>>>;
