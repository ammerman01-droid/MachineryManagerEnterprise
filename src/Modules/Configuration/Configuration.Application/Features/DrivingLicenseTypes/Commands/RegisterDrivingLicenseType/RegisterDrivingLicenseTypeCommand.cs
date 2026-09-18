using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RegisterDrivingLicenseType;

/// <summary>Registers a new Driving License Type option within a Holding.</summary>
public sealed record RegisterDrivingLicenseTypeCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;