using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.Usage.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// EF Core implementation of <see cref="IUsageProvisioningService"/>
/// (chat, 2026-09-29). Registers a Meter Device for a newly created
/// Asset and installs it immediately, so the Asset shows up ready for
/// its first shift reading without a separate manual step. Best-effort
/// per the interface contract: any failure is logged and swallowed —
/// see the caller (<c>RegisterAssetCommandHandler</c>) for the
/// surrounding try/catch.
/// </summary>
public sealed class UsageProvisioningService : IUsageProvisioningService
{
    private readonly UsageDbContext _dbContext;
    private readonly IMeterDeviceRepository _meterDeviceRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<UsageProvisioningService> _logger;

    /// <summary>Initializes a new instance of the <see cref="UsageProvisioningService"/> class.</summary>
    /// <param name="dbContext">The Usage module's persistence context, used to commit the new device.</param>
    /// <param name="meterDeviceRepository">The Meter Device repository.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised <c>MeterInstalled</c> event.</param>
    /// <param name="logger">Logger used to record provisioning failures without surfacing them to the caller.</param>
    public UsageProvisioningService(
        UsageDbContext dbContext,
        IMeterDeviceRepository meterDeviceRepository,
        IDateTimeProvider dateTimeProvider,
        ILogger<UsageProvisioningService> logger)
    {
        _dbContext = dbContext;
        _meterDeviceRepository = meterDeviceRepository;
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
        var registerResult = global::Usage.Domain.MeterDevice.Register(organizationId, meterReadingUnit, dailyCapOverride: null);

        if (registerResult.IsFailure)
        {
            _logger.LogError(
                "Could not register a Meter Device for new Asset {AssetId}: {ErrorCode} — {ErrorMessage}",
                assetId, registerResult.Error.Code, registerResult.Error.Message);
            return;
        }

        var device = registerResult.Value;

        var installResult = device.Install(global::Usage.Domain.UsageOwnerType.Asset, assetId, meterReadingUnit, _dateTimeProvider);

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
}
