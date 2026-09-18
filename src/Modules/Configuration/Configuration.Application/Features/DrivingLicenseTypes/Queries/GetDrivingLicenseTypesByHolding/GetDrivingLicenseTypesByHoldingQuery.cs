using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Queries.GetDrivingLicenseTypesByHolding;

/// <summary>Retrieves the list of Driving License Type options defined for a Holding.</summary>
public sealed record GetDrivingLicenseTypesByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<DrivingLicenseTypeDto>>>;