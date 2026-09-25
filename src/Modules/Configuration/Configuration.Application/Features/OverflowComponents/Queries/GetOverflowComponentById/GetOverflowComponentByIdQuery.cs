using MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Queries.GetOverflowComponentById;

/// <summary>Retrieves a single Overflow Component by its identifier.</summary>
public sealed record GetOverflowComponentByIdQuery(Guid OverflowComponentId) : IRequest<Result<OverflowComponentDto>>;
