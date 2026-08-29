using MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Queries.GetUnitsOfMeasurementByOrganization;

/// <summary>Query to retrieve every Unit of Measurement registered for an Organization.</summary>
public sealed record GetUnitsOfMeasurementByOrganizationQuery(Guid OrganizationId)
    : IRequest<Result<IReadOnlyList<UnitOfMeasurementDto>>>;