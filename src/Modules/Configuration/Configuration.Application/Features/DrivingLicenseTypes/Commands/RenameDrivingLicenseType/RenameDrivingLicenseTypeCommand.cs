using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RenameDrivingLicenseType;

/// <summary>Renames an existing Driving License Type.</summary>
public sealed record RenameDrivingLicenseTypeCommand(Guid DrivingLicenseTypeId, string Name) : IRequest<Result>;