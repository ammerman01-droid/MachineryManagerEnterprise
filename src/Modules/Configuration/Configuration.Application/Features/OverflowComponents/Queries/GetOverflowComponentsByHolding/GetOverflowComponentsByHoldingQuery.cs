using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Queries.GetOverflowComponentsByHolding;

/// <summary>Retrieves the list of Overflow Component options defined for a Holding.</summary>
public sealed record GetOverflowComponentsByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<OverflowComponentDto>>>;
