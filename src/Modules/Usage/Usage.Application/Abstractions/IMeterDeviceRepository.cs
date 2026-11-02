using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Dtos;

namespace MachineryManagerEnterprise.Usage.Application.Abstractions;

/// <summary>Repository contract for the <see cref="global::Usage.Domain.MeterDevice"/> aggregate.</summary>
public interface IMeterDeviceRepository : IRepository<global::Usage.Domain.MeterDevice, global::Usage.Domain.MeterDeviceId>
{
    /// <summary>
    /// Lists an Organization's Meter Devices, paged — same
    /// materialize-with-<c>AsNoTracking</c>-then-project convention as
    /// <c>AssetRepository.SearchAsync</c>. Backs the device list page and the
    /// replacement-device picker of the rebase dialog (chat, 2026-10-28).
    /// </summary>
    /// <param name="organizationId">The Organization to list devices for.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Maximum number of devices per page.</param>
    /// <param name="ownerId">When set, only devices currently installed on this owner are returned.</param>
    /// <param name="unit">When set, only devices reading in this unit are returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<SearchMeterDevicesResponse> SearchAsync(
        Guid organizationId,
        int page,
        int pageSize,
        Guid? ownerId = null,
        MeterReadingUnit? unit = null,
        CancellationToken cancellationToken = default);

    /// <summary>Checks whether a device with the given name already exists in the Organization.</summary>
    /// <param name="organizationId">The Organization to check within.</param>
    /// <param name="name">The (already trimmed) name to look for.</param>
    /// <param name="excludeMeterDeviceId">A device to ignore — the one being renamed — or <see langword="null"/>.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<bool> NameExistsAsync(
        Guid organizationId,
        string name,
        Guid? excludeMeterDeviceId,
        CancellationToken cancellationToken = default);

    /// <summary>Lists the distinct reading units of the devices currently installed on an owner, ignoring one device.</summary>
    /// <param name="ownerType">The kind of owner.</param>
    /// <param name="ownerId">The owner's identifier.</param>
    /// <param name="excludeMeterDeviceId">The device being installed, which must not count against itself.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task<IReadOnlyList<MeterReadingUnit>> ListUnitsOnOwnerAsync(
        global::Usage.Domain.UsageOwnerType ownerType,
        Guid ownerId,
        Guid excludeMeterDeviceId,
        CancellationToken cancellationToken = default);
}
