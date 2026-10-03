using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.SearchMeterDevices;

/// <summary>Query to list an Organization's Meter Devices, paged — backs the device list page.</summary>
/// <param name="OrganizationId">The Organization to list devices for.</param>
/// <param name="Page">1-based page number. Defaults to the first page.</param>
/// <param name="PageSize">Maximum number of devices per page. Defaults to 50.</param>
public sealed record SearchMeterDevicesQuery(
    Guid OrganizationId,
    int Page = 1,
    int PageSize = 50) : IRequest<Result<SearchMeterDevicesResponse>>;
