using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Queries.GetDrivingLicenseTypeById;

/// <summary>Retrieves a single Driving License Type by its identifier.</summary>
public sealed record GetDrivingLicenseTypeByIdQuery(Guid DrivingLicenseTypeId) : IRequest<Result<DrivingLicenseTypeDto>>;