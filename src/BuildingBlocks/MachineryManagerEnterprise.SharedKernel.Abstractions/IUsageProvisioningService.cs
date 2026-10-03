namespace MachineryManagerEnterprise.SharedKernel.Abstractions;

/// <summary>
/// Cross-module, write-side hook into the Usage module, called by
/// <c>RegisterAssetCommandHandler</c> immediately after a new Asset is
/// persisted (chat, 2026-09-29), so the Asset module never depends on
/// Usage.Domain or Usage.Application directly (Modular Monolith
/// boundary — same pattern as <see cref="IWorkCalendarLookupService"/>).
/// Best-effort: a failure here is logged and never rolls back or fails
/// the Asset registration itself, since the counter can always be
/// provisioned later from the Meter Devices screen.
/// </summary>
public interface IUsageProvisioningService
{
    /// <summary>
    /// Provisions a Meter Device (and, once a reading is first
    /// registered, its Usage Ledger) for a newly registered Asset.
    /// </summary>
    /// <param name="assetId">The identifier of the newly registered Asset.</param>
    /// <param name="organizationId">The Organization the Asset belongs to.</param>
    /// <param name="meterReadingUnit">The Asset's configured meter reading unit (mandatory as of chat, 2026-09-29).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    Task ProvisionForNewAssetAsync(
        Guid assetId,
        Guid organizationId,
        MeterReadingUnit meterReadingUnit,
        CancellationToken cancellationToken = default);
}
