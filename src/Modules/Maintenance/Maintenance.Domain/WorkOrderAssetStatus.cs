namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>
/// The Asset's resulting operational status as recorded on a Work Order
/// (chat, 2026-09-22): a failure may just be reported without causing a
/// stoppage (<see cref="Active"/>), or it may take the Asset out of
/// service. Deliberately a LOCAL, two-value enum rather than a
/// reference to <c>Asset.Domain.AssetStatus</c> — Maintenance.Domain
/// must not depend on another module's Domain layer (Modular Monolith
/// rule). The Maintenance.Application layer maps this to the real
/// Asset status via <c>IAssetStatusUpdateService</c> when a Work Order
/// is registered.
/// </summary>
/// <remarks>Persisted by name — never rename a member without a data migration.</remarks>
public enum WorkOrderAssetStatus
{
    /// <summary>فعال — the failure was reported but did not stop the Asset.</summary>
    Active = 1,

    /// <summary>خارج از سرویس — the failure stopped the Asset.</summary>
    OutOfService = 2,
}
