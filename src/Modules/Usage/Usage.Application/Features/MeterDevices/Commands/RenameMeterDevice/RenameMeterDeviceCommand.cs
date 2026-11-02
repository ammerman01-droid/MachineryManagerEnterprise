using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RenameMeterDevice;

/// <summary>Command to rename a Meter Device; the new name must be unique within the device's Organization.</summary>
/// <param name="MeterDeviceId">The device to rename.</param>
/// <param name="Name">The new human-readable name.</param>
public sealed record RenameMeterDeviceCommand(Guid MeterDeviceId, string Name) : IRequest<Result>;
