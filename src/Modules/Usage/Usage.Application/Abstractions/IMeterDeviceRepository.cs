using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;

namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Usage.Domain.MeterDevice"/> aggregate.</summary>
public interface IMeterDeviceRepository : IRepository<global::Usage.Domain.MeterDevice, global::Usage.Domain.MeterDeviceId>
{
    /// <summary>
    /// Lists an Organization's Meter Devices, paged — same
    /// materialize-with-<c>AsNoTracking</c>-then-project convention as
    /// <c>AssetRepository.SearchAsync</c>. Backs the device list page
    /// (chat, 2026-09-2x).
    /// </summary>
    /// <param name="organizationId">The Organization to list devices for.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Maximum number of devices per page.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<SearchMeterDevicesResponse> SearchAsync(
        Guid organizationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
