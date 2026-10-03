namespace MachineryManagerEnterprise.Usage.Infrastructure.Persistence;

/// <summary>
/// The Usage module's own small settings row for the system-wide
/// default daily cap on Operational Usage (BR-045), one row per
/// <see cref="global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit"/>.
/// Deliberately plain Infrastructure state rather than a Domain
/// concept — it carries no business invariants of its own beyond "a
/// positive number per unit" — per the design note on
/// <see cref="global::MachineryManagerEnterprise.Usage.Application.Abstractions.IUsageCapPolicy"/>
/// (chat, 2026-09-12).
/// </summary>
public sealed class UsageCapSetting
{
    /// <summary>Gets the reading unit this default applies to (Hour, Kilometer, or Mile). The table's key.</summary>
    public global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit { get; private set; }

    /// <summary>Gets the system-wide default daily cap for <see cref="Unit"/> (BR-045).</summary>
    public decimal DefaultDailyCap { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private UsageCapSetting()
    {
    }

    /// <summary>Creates a settings row — used only for the initial seed data (see <c>UsageCapSettingConfiguration</c>) and by <c>UsageCapPolicy</c> when writing an administrative change.</summary>
    public UsageCapSetting(global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit unit, decimal defaultDailyCap)
    {
        Unit = unit;
        DefaultDailyCap = defaultDailyCap;
    }

    /// <summary>Updates the default daily cap for this unit. Administrative change; this type raises no Domain Events, since it is not a Domain concept (see class remarks).</summary>
    public void UpdateDefaultDailyCap(decimal newValue) => DefaultDailyCap = newValue;
}
