using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorById;

/// <summary>Retrieves a single Color by its identifier (used to pre-fill the Edit form).</summary>
/// <param name="Id">The identifier of the Color to retrieve.</param>
public sealed record GetColorByIdQuery(Guid Id) : IRequest<Result<ColorDto>>;
