using MachineryManager.Asset.Application.Features.Colors.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Colors.Queries.GetColorsByOrganization;

/// <summary>Query to retrieve every Color registered for an Organization (used to populate selection lists).</summary>
public sealed record GetColorsByOrganizationQuery(Guid OrganizationId) : IRequest<Result<IReadOnlyList<ColorDto>>>;