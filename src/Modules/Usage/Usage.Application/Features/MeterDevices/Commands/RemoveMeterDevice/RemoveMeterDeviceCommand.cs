using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RemoveMeterDevice;

/// <summary>
/// Command to remove a Meter Device from its current owner
/// (Installed/Operational/Failed → Removed). The device's established
/// <c>Unit</c> and its full reading history are preserved (BR-011/BR-013)
/// so it can be re-installed later without loss.
/// </summary>
public sealed record RemoveMeterDeviceCommand(
    Guid MeterDeviceId) : IRequest<Result>;
