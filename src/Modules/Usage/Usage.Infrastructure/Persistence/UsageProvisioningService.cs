using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUsageProvisioningService"/>
/// (chat, 2026-09-29). Registers a Meter Device for a newly created
/// Asset and installs it immediately, so the Asset shows up ready for
/// its first shift reading without a separate manual step. The device is
/// named after its kind and the Asset's code (chat, 2026-10-28) so it is
/// recognizable in every list. Best-effort per the interface contract:
/// any failure is logged and swallowed — see the caller
/// (<c>RegisterAssetCommandHandler</c>) for the surrounding try/catch.
/// </summary>
public sealed class UsageProvisioningService : IUsageProvisioningService
{
    private readonly UsageDbContext _dbContext;
    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<UsageProvisioningService> _logger;

    /// <summary>Initializes a new instance of the <see cref="UsageProvisioningService"/> class.</summary>
    /// <param name="dbContext">The Usage module's persistence context, used to commit the new device.</param>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to name the device after the Asset's code.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised <c>MeterInstalled</c> event.</param>
    /// <param name="logger">Logger used to record provisioning failures without surfacing them to the caller.</param>
    public UsageProvisioningService(
        UsageDbContext dbContext,
        IMeterDeviceRepository meterDeviceRepository,
        IAssetLookupService assetLookupService,
        IDateTimeProvider dateTimeProvider,
        ILogger<UsageProvisioningService> logger)
    {
        _dbContext = dbContext;
        _meterDeviceRepository = meterDeviceRepository;
        _assetLookupService = assetLookupService;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ProvisionForNewAssetAsync(
        Guid assetId,
        Guid organizationId,
        MeterReadingUnit meterReadingUnit,
        CancellationToken cancellationToken = default)
    {
        var name = await BuildDeviceNameAsync(assetId, meterReadingUnit, cancellationToken);

        var registerResult = global::Usage.Domain.MeterDevice.Register(organizationId, name, meterReadingUnit, dailyCapOverride: null);

        if (registerResult.IsFailure)
        {
            _logger.LogError(
                "Could not register a Meter Device for new Asset {AssetId}: {ErrorCode} — {ErrorMessage}",
                assetId, registerResult.Error.Code, registerResult.Error.Message);
            return;
        }

        var device = registerResult.Value;

        var installResult = device.Install(global::Usage.Domain.UsageOwnerType.Asset, assetId, _dateTimeProvider);

        if (installResult.IsFailure)
        {
            _logger.LogError(
                "Could not install the auto-provisioned Meter Device on new Asset {AssetId}: {ErrorCode} — {ErrorMessage}",
                assetId, installResult.Error.Code, installResult.Error.Message);
            return;
        }

        _meterDeviceRepository.Add(device);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> BuildDeviceNameAsync(Guid assetId, MeterReadingUnit unit, CancellationToken cancellationToken)
    {
        var kind = unit switch
        {
            MeterReadingUnit.Hour => "ساعت‌شمار",
            MeterReadingUnit.Kilometer => "کیلومترشمار",
            MeterReadingUnit.Mile => "مایل‌شمار",
            _ => "کنتور",
        };

        try
        {
            var infos = await _assetLookupService.GetDisplayInfosAsync([assetId], cancellationToken);

            if (infos.TryGetValue(assetId, out var info) && !string.IsNullOrWhiteSpace(info.Code))
            {
                return $"{kind} {info.Code}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not resolve the code of Asset {AssetId} to name its auto-provisioned Meter Device.", assetId);
        }

        return $"{kind} {assetId.ToString("N")[..8]}";
    }
}
