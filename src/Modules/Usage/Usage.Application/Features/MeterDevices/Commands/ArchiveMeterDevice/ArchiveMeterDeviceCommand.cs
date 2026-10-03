using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.ArchiveMeterDevice;

/// <summary>
/// Command to permanently close the lifecycle of a removed Meter
/// Device (Removed → Archived). This is the terminal state; the
/// device's full reading history remains accessible afterward.
/// </summary>
public sealed record ArchiveMeterDeviceCommand(
    Guid MeterDeviceId) : IRequest<Result>;
