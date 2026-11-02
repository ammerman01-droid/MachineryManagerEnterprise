using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.SearchMeterDevices;

/// <summary>Query to list an Organization's Meter Devices, paged — backs the device list page and the rebase dialog's replacement-device picker.</summary>
/// <param name="OrganizationId">The Organization to list devices for.</param>
/// <param name="Page">1-based page number. Defaults to the first page.</param>
/// <param name="PageSize">Maximum number of devices per page. Defaults to 50.</param>
/// <param name="OwnerId">When set, only devices currently installed on this owner are returned.</param>
/// <param name="Unit">When set, only devices reading in this unit are returned.</param>
public sealed record SearchMeterDevicesQuery(
    Guid OrganizationId,
    int Page = 1,
    int PageSize = 50,
    Guid? OwnerId = null,
    MeterReadingUnit? Unit = null) : IRequest<Result<SearchMeterDevicesResponse>>;
