namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;

/// <summary>One row of a Meter Device search/detail result, projected directly off the persistence store.</summary>
/// <param name="Id">The device's identifier.</param>
/// <param name="OrganizationId">The Organization this device belongs to.</param>
/// <param name="Unit">The reading unit this device measures in, once its first reading has been recorded — <see langword="null"/> before then (BR-005).</param>
/// <param name="Status">The device's current lifecycle status, by name (Registered, Installed, Operational, Failed, Removed, or Archived).</param>
/// <param name="OwnerType">The kind of owner this device is mounted on, by name (Asset or Component), or <see langword="null"/> if not currently mounted.</param>
/// <param name="OwnerId">The owner's identifier, or <see langword="null"/> if not currently mounted.</param>
/// <param name="DailyCapOverride">This device's own override of the system-wide daily cap (BR-045), or <see langword="null"/> to use the default.</param>
public sealed record MeterDeviceListItemDto(
    Guid Id,
    Guid OrganizationId,
    string? Unit,
    string Status,
    string? OwnerType,
    Guid? OwnerId,
    decimal? DailyCapOverride);

/// <summary>A page of Meter Device search results, with the total count of matching rows across every page.</summary>
/// <param name="Items">The rows on this page.</param>
/// <param name="TotalCount">The total number of rows matching the search, across every page.</param>
public sealed record SearchMeterDevicesResponse(IReadOnlyList<MeterDeviceListItemDto> Items, int TotalCount);
