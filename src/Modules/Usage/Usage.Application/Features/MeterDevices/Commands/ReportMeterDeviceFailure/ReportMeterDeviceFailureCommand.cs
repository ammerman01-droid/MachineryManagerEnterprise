using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.ReportMeterDeviceFailure;

/// <summary>
/// Command to mark an installed Meter Device as malfunctioning
/// (Installed/Operational → Failed, BR-014). Once reported, the device
/// stops accepting readings until <c>RecoverMeterDevice</c> restores it.
/// </summary>
public sealed record ReportMeterDeviceFailureCommand(
    Guid MeterDeviceId) : IRequest<Result>;
