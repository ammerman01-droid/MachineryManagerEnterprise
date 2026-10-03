using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.GetMeterDeviceById;

/// <summary>Query to fetch a single Meter Device by id — backs the device details page.</summary>
/// <param name="MeterDeviceId">The device's identifier.</param>
public sealed record GetMeterDeviceByIdQuery(Guid MeterDeviceId) : IRequest<Result<MeterDeviceListItemDto>>;
