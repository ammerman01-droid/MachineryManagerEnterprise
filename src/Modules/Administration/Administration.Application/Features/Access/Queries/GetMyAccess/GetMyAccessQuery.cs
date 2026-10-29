using MachineryManagerEnterprise.Administration.Application.Features.Access.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Access.Queries.GetMyAccess;

/// <summary>Retrieves the signed-in user's level, position and effective permissions (drives the UI).</summary>
public sealed record GetMyAccessQuery : IRequest<Result<MyAccessDto>>;
