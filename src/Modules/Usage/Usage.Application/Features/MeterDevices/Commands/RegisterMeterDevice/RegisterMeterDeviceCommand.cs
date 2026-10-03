using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RegisterMeterDevice;

/// <summary>
/// Command to register a new Meter Device within an Organization (BR-041).
/// The reading <paramref name="Unit"/> is fixed at registration and can
/// never change afterward (chat, 2026-09-29); the device starts unowned.
/// </summary>
/// <param name="OrganizationId">The Organization this device belongs to.</param>
/// <param name="Unit">The unit this device reads in; fixed for its lifetime.</param>
/// <param name="DailyCapOverride">Optional owner-specific override of the daily cap (BR-045).</param>
public sealed record RegisterMeterDeviceCommand(
    Guid OrganizationId,
    MeterReadingUnit Unit,
    decimal? DailyCapOverride) : IRequest<Result<Guid>>;