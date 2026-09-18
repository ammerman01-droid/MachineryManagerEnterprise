using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.DeleteDrivingLicenseType;

/// <summary>Deletes a Driving License Type, provided it is not referenced by any Personnel record.</summary>
public sealed record DeleteDrivingLicenseTypeCommand(Guid DrivingLicenseTypeId) : IRequest<Result>;