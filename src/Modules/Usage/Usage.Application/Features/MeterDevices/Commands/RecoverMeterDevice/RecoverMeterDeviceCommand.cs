using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RecoverMeterDevice;

/// <summary>
/// Command to mark a previously failed Meter Device as repaired and
/// ready to resume readings (Failed → Installed). The device is not
/// re-confirmed as Operational until its next successful reading.
/// </summary>
public sealed record RecoverMeterDeviceCommand(
    Guid MeterDeviceId) : IRequest<Result>;
