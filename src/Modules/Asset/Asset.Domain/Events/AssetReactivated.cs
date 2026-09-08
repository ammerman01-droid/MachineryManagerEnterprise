using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>
/// Raised when a previously Inactive Asset returns to operation
/// (Inactive → Operational). Distinct from <see cref="AssetActivated"/>,
/// which covers the initial Commissioned → Operational transition —
/// keeping them separate preserves an accurate audit trail of first
/// activation vs. later reactivations (chat, 2026-08-27).
/// </summary>
public sealed class AssetReactivated : IDomainEvent
{
    /// <summary>Gets the identifier of the reactivated asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetReactivated"/> class.</summary>
    /// <param name="assetId">The identifier of the reactivated asset.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetReactivated(AssetId assetId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}