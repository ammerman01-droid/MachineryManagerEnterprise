using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.InstallMeterDevice;

/// <summary>Command to mount a registered Meter Device on an owner (an Asset or a Tracked Component).</summary>
public sealed record InstallMeterDeviceCommand(
    Guid MeterDeviceId,
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId) : IRequest<Result>;
