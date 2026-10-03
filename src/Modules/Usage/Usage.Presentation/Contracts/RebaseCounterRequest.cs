namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for explicitly rebasing a Usage Ledger's counter — the device was replaced or its counter reset (chat, 2026-09-29).</summary>
/// <param name="NewMeterDeviceId">The replacement device, which must already be installed on this Ledger's owner.</param>
/// <param name="NewRawReadingValue">The new baseline raw value shown on the (possibly new) device.</param>
/// <param name="TargetDate">The date to record the rebase on; must be after the Ledger's last recorded entry and no later than today.</param>
public sealed record RebaseCounterRequest(
    Guid NewMeterDeviceId,
    decimal NewRawReadingValue,
    DateOnly TargetDate);
