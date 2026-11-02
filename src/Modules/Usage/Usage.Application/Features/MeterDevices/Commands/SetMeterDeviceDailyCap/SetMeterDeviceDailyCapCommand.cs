using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.SetMeterDeviceDailyCap;

/// <summary>Command to set or clear a Meter Device's own override of the daily Operational Usage cap (BR-045).</summary>
/// <param name="MeterDeviceId">The device to update.</param>
/// <param name="DailyCapOverride">The new override, or <see langword="null"/> to fall back to the system default.</param>
public sealed record SetMeterDeviceDailyCapCommand(Guid MeterDeviceId, decimal? DailyCapOverride) : IRequest<Result>;
